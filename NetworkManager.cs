using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace IPConfiger
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public class NetworkManager
    {
        /// <summary>
        /// 获取所有网络适配器
        /// </summary>
        public List<NetworkAdapter> GetNetworkAdapters()
        {
            var adapters = new List<NetworkAdapter>();

            try
            {
                var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                    .Where(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                    .Where(ni => ni.OperationalStatus == OperationalStatus.Up ||
                                ni.OperationalStatus == OperationalStatus.Down);

                foreach (var ni in networkInterfaces)
                {
                    var adapter = new NetworkAdapter
                    {
                        Name = ni.Name,
                        Description = ni.Description,
                        IsUp = ni.OperationalStatus == OperationalStatus.Up
                    };

                    // 获取IP信息
                    var ipProps = ni.GetIPProperties();
                    var unicastAddresses = ipProps.UnicastAddresses
                        .Where(ua => ua.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .ToList();

                    if (unicastAddresses.Any())
                    {
                        var primaryAddress = unicastAddresses.First();
                        adapter.CurrentIP = primaryAddress.Address.ToString();
                        adapter.CurrentSubnet = primaryAddress.IPv4Mask?.ToString() ?? "";
                    }

                    // 获取网关信息
                    var gateways = ipProps.GatewayAddresses
                        .Where(ga => ga.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        .ToList();

                    if (gateways.Any())
                    {
                        adapter.CurrentGateway = gateways.First().Address.ToString();
                    }

                    adapters.Add(adapter);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取网络适配器失败: {ex.Message}");
            }

            return adapters;
        }

        /// <summary>
        /// 应用网络配置
        /// </summary>
        public async Task<bool> ApplyNetworkConfig(NetworkConfig config)
        {
            try
            {
                if (config.UseDHCP)
                {
                    return await SetDHCP(config.AdapterName);
                }
                else
                {
                    return await SetStaticIP(config);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"应用网络配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置为DHCP
        /// </summary>
        private async Task<bool> SetDHCP(string adapterName)
        {
            try
            {
                // 设置IP为DHCP
                var ipResult = await RunNetshCommand($"interface ip set address \"{adapterName}\" dhcp");
                if (!ipResult)
                    return false;

                // 设置DNS为DHCP
                var dnsResult = await RunNetshCommand($"interface ip set dns \"{adapterName}\" dhcp");
                return dnsResult;
            }
            catch (Exception ex)
            {
                throw new Exception($"设置DHCP失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置静态IP
        /// </summary>
        private async Task<bool> SetStaticIP(NetworkConfig config)
        {
            try
            {
                // 设置静态IP
                var ipCommand = $"interface ip set address \"{config.AdapterName}\" static {config.IPAddress} {config.SubnetMask}";
                if (!string.IsNullOrEmpty(config.Gateway))
                {
                    ipCommand += $" {config.Gateway}";
                }

                var ipResult = await RunNetshCommand(ipCommand);
                if (!ipResult)
                    return false;

                // 设置DNS
                if (!string.IsNullOrEmpty(config.DNS1))
                {
                    var dns1Result = await RunNetshCommand($"interface ip set dns \"{config.AdapterName}\" static {config.DNS1}");
                    if (!dns1Result)
                        return false;

                    // 设置备用DNS
                    if (!string.IsNullOrEmpty(config.DNS2))
                    {
                        var dns2Result = await RunNetshCommand($"interface ip add dns \"{config.AdapterName}\" {config.DNS2} index=2");
                        if (!dns2Result)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"设置静态IP失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 执行netsh命令
        /// </summary>
        private async Task<bool> RunNetshCommand(string arguments)
        {
            try
            {
                var processInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using var process = Process.Start(processInfo);
                if (process == null)
                    return false;

                await process.WaitForExitAsync();
                return process.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取当前网络配置
        /// </summary>
        public NetworkConfig GetCurrentNetworkConfig(string adapterName)
        {
            var config = new NetworkConfig
            {
                AdapterName = adapterName,
                Name = $"当前配置_{adapterName}"
            };

            try
            {
                var adapter = GetNetworkAdapters().FirstOrDefault(a => a.Name == adapterName);
                if (adapter != null)
                {
                    config.IPAddress = adapter.CurrentIP;
                    config.SubnetMask = adapter.CurrentSubnet;
                    config.Gateway = adapter.CurrentGateway;
                    
                    // 判断是否为DHCP（简单判断，实际可能需要更复杂的逻辑）
                    config.UseDHCP = string.IsNullOrEmpty(adapter.CurrentIP) || adapter.CurrentIP.StartsWith("169.254");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取当前网络配置失败: {ex.Message}");
            }

            return config;
        }
    }
}