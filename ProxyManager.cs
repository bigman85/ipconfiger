using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace IPConfiger
{
    /// <summary>
    /// 代理服务器管理器
    /// </summary>
    public class ProxyManager
    {
        private const string INTERNET_SETTINGS_KEY = @"Software\Microsoft\Windows\CurrentVersion\Internet Settings";

        /// <summary>
        /// 应用代理配置
        /// </summary>
        public async Task<bool> ApplyProxyConfig(ProxyConfig config)
        {
            try
            {
                if (config.UseProxy)
                {
                    return await SetProxy(config);
                }
                else
                {
                    return await DisableProxy();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"应用代理配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 设置代理服务器
        /// </summary>
        private async Task<bool> SetProxy(ProxyConfig config)
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(INTERNET_SETTINGS_KEY, true);
                if (key == null)
                {
                    throw new Exception("无法访问Internet设置注册表项");
                }

                // 启用代理
                key.SetValue("ProxyEnable", 1, RegistryValueKind.DWord);
                
                // 设置代理服务器地址
                var proxyAddress = $"{config.ProxyServer}:{config.ProxyPort}";
                key.SetValue("ProxyServer", proxyAddress, RegistryValueKind.String);
                
                // 设置代理覆盖（绕过列表）
                var bypassList = config.ProxyBypassList;
                if (config.ProxyBypassLocal && !string.IsNullOrEmpty(bypassList))
                {
                    bypassList += ";";
                }
                if (config.ProxyBypassLocal)
                {
                    bypassList += "<local>";
                }
                
                if (!string.IsNullOrEmpty(bypassList))
                {
                    key.SetValue("ProxyOverride", bypassList, RegistryValueKind.String);
                }
                else
                {
                    key.DeleteValue("ProxyOverride", false);
                }

                // 刷新Internet设置
                await RefreshInternetSettings();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"设置代理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 禁用代理服务器
        /// </summary>
        private async Task<bool> DisableProxy()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(INTERNET_SETTINGS_KEY, true);
                if (key == null)
                {
                    throw new Exception("无法访问Internet设置注册表项");
                }

                // 禁用代理
                key.SetValue("ProxyEnable", 0, RegistryValueKind.DWord);
                
                // 刷新Internet设置
                await RefreshInternetSettings();
                
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"禁用代理失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取当前代理配置
        /// </summary>
        public ProxyConfig GetCurrentProxyConfig()
        {
            var config = new ProxyConfig();

            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(INTERNET_SETTINGS_KEY, false);
                if (key == null)
                {
                    return config;
                }

                // 检查代理是否启用
                var proxyEnabled = key.GetValue("ProxyEnable");
                config.UseProxy = proxyEnabled != null && (int)proxyEnabled == 1;

                if (config.UseProxy)
                {
                    // 获取代理服务器地址
                    var proxyServer = key.GetValue("ProxyServer") as string;
                    if (!string.IsNullOrEmpty(proxyServer))
                    {
                        var parts = proxyServer.Split(':');
                        if (parts.Length >= 1)
                        {
                            config.ProxyServer = parts[0];
                        }
                        if (parts.Length >= 2 && int.TryParse(parts[1], out int port))
                        {
                            config.ProxyPort = port;
                        }
                    }

                    // 获取绕过列表
                    var proxyOverride = key.GetValue("ProxyOverride") as string;
                    if (!string.IsNullOrEmpty(proxyOverride))
                    {
                        config.ProxyBypassLocal = proxyOverride.Contains("<local>");
                        config.ProxyBypassList = proxyOverride.Replace("<local>", "").Replace(";", "").Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"获取当前代理配置失败: {ex.Message}");
            }

            return config;
        }

        /// <summary>
        /// 刷新Internet设置
        /// </summary>
        private async Task RefreshInternetSettings()
        {
            try
            {
                // 使用netsh命令刷新代理设置
                var processInfo = new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments = "winhttp reset proxy",
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using var process = Process.Start(processInfo);
                if (process != null)
                {
                    await process.WaitForExitAsync();
                }

                // 通知系统代理设置已更改
                await Task.Run(() =>
                {
                    System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_SETTINGS_CHANGED, IntPtr.Zero, 0));
                    System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(
                        InternetSetOption(IntPtr.Zero, INTERNET_OPTION_REFRESH, IntPtr.Zero, 0));
                });
            }
            catch
            {
                // 忽略刷新错误，不影响主要功能
            }
        }

        #region Win32 API
        private const int INTERNET_OPTION_SETTINGS_CHANGED = 39;
        private const int INTERNET_OPTION_REFRESH = 37;

        [System.Runtime.InteropServices.DllImport("wininet.dll", SetLastError = true)]
        private static extern int InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int dwBufferLength);
        #endregion
    }
}