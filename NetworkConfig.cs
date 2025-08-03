using System;
using System.Collections.Generic;

namespace IPConfiger
{
    /// <summary>
    /// 网络配置类
    /// </summary>
    public class NetworkConfig
    {
        public string Name { get; set; } = string.Empty;
        public string AdapterName { get; set; } = string.Empty;
        public bool UseDHCP { get; set; } = true;
        public string IPAddress { get; set; } = string.Empty;
        public string SubnetMask { get; set; } = string.Empty;
        public string Gateway { get; set; } = string.Empty;
        public string DNS1 { get; set; } = string.Empty;
        public string DNS2 { get; set; } = string.Empty;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime ModifiedTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 网络适配器信息类
    /// </summary>
    public class NetworkAdapter
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsUp { get; set; }
        public string CurrentIP { get; set; } = string.Empty;
        public string CurrentSubnet { get; set; } = string.Empty;
        public string CurrentGateway { get; set; } = string.Empty;
    }
}