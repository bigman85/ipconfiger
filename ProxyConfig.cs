using System;

namespace IPConfiger
{
    /// <summary>
    /// 代理类型枚举
    /// </summary>
    public enum ProxyType
    {
        HTTP,
        HTTPS,
        SOCKS4,
        SOCKS5
    }

    /// <summary>
    /// 代理配置类
    /// </summary>
    public class ProxyConfig
    {
        public string Name { get; set; } = string.Empty;
        public bool UseProxy { get; set; } = false;
        public ProxyType ProxyType { get; set; } = ProxyType.HTTP;
        public string ProxyServer { get; set; } = string.Empty;
        public int ProxyPort { get; set; } = 8080;
        public bool ProxyRequiresAuth { get; set; } = false;
        public string ProxyUsername { get; set; } = string.Empty;
        public string ProxyPassword { get; set; } = string.Empty;
        public string ProxyBypassList { get; set; } = string.Empty;
        public bool ProxyBypassLocal { get; set; } = true;
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        public DateTime ModifiedTime { get; set; } = DateTime.Now;
    }
}