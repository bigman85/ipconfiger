# IP配置管理器

一个用于管理Windows网络配置和代理设置的桌面应用程序。

## 功能特性

- 网络配置管理：支持DHCP和静态IP配置
- 代理服务器配置：支持HTTP、HTTPS、SOCKS4、SOCKS5代理
- 配置快速切换：一键应用预设的网络配置
- 配置导入导出：支持配置文件的备份和恢复
- 管理员权限：自动请求管理员权限以修改网络设置

## 系统要求

- Windows 10/11
- .NET 9.0 Runtime
- 管理员权限

## 技术栈

- C# / .NET 9.0
- Windows Forms
- Newtonsoft.Json
- System.Management

## 编译和运行

```bash
# 克隆仓库
git clone https://github.com/bigman85/ipconfiger.git
cd ipconfiger

# 编译项目
dotnet build

# 运行应用程序
dotnet run
```

## 许可证

Copyright © 2024 IPConfiger
