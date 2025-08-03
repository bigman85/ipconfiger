using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace IPConfiger
{
    /// <summary>
    /// 配置管理器
    /// </summary>
    public class ConfigManager
    {
        private const string CONFIG_FILE = "configs.json";
        private List<NetworkConfig> _configs;

        public ConfigManager()
        {
            _configs = new List<NetworkConfig>();
            LoadConfigs();
        }

        /// <summary>
        /// 获取所有配置
        /// </summary>
        public List<NetworkConfig> GetConfigs()
        {
            return _configs.ToList();
        }

        /// <summary>
        /// 保存配置到文件
        /// </summary>
        public void SaveConfigs()
        {
            try
            {
                var json = JsonConvert.SerializeObject(_configs, Formatting.Indented);
                File.WriteAllText(CONFIG_FILE, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"保存配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从文件加载配置
        /// </summary>
        private void LoadConfigs()
        {
            try
            {
                if (File.Exists(CONFIG_FILE))
                {
                    var json = File.ReadAllText(CONFIG_FILE);
                    _configs = JsonConvert.DeserializeObject<List<NetworkConfig>>(json) ?? new List<NetworkConfig>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"加载配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        public void AddConfig(NetworkConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            if (_configs.Any(c => c.Name == config.Name))
                throw new Exception("配置名称已存在");

            config.CreatedTime = DateTime.Now;
            config.ModifiedTime = DateTime.Now;
            _configs.Add(config);
            SaveConfigs();
        }

        /// <summary>
        /// 更新配置
        /// </summary>
        public void UpdateConfig(NetworkConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var existingConfig = _configs.FirstOrDefault(c => c.Name == config.Name);
            if (existingConfig == null)
                throw new Exception("配置不存在");

            config.CreatedTime = existingConfig.CreatedTime;
            config.ModifiedTime = DateTime.Now;
            
            var index = _configs.IndexOf(existingConfig);
            _configs[index] = config;
            SaveConfigs();
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        public void DeleteConfig(string name)
        {
            var config = _configs.FirstOrDefault(c => c.Name == name);
            if (config != null)
            {
                _configs.Remove(config);
                SaveConfigs();
            }
        }

        /// <summary>
        /// 获取指定名称的配置
        /// </summary>
        public NetworkConfig? GetConfig(string name)
        {
            return _configs.FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// 导出配置到文件
        /// </summary>
        public void ExportConfigs(string filePath)
        {
            try
            {
                var json = JsonConvert.SerializeObject(_configs, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"导出配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 从文件导入配置
        /// </summary>
        public void ImportConfigs(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    throw new FileNotFoundException("文件不存在");

                var json = File.ReadAllText(filePath);
                var importedConfigs = JsonConvert.DeserializeObject<List<NetworkConfig>>(json);
                
                if (importedConfigs != null)
                {
                    foreach (var config in importedConfigs)
                    {
                        if (!_configs.Any(c => c.Name == config.Name))
                        {
                            _configs.Add(config);
                        }
                    }
                    SaveConfigs();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"导入配置失败: {ex.Message}");
            }
        }
    }
}