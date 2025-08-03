using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace IPConfiger
{
    /// <summary>
    /// 主窗体
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly ConfigManager _configManager;
        private readonly ProxyConfigManager _proxyConfigManager;
        private readonly NetworkManager _networkManager;
        private readonly ProxyManager _proxyManager;

        // 网络适配器相关控件
        private Label _adapterLabel;
        private ComboBox _adapterComboBox;
        private Button _refreshButton;
        private Button _getCurrentButton;
        private Button _getProxyButton;

        // 网络配置相关控件
        private GroupBox _configGroupBox;
        private ListBox _configListBox;
        private Button _addButton;
        private Button _editButton;
        private Button _deleteButton;
        private Button _applyButton;

        // 代理配置相关控件
        private GroupBox _proxyConfigGroupBox;
        private ListBox _proxyConfigListBox;
        private Button _addProxyButton;
        private Button _editProxyButton;
        private Button _deleteProxyButton;
        private Button _applyProxyButton;

        // 操作相关控件
        private GroupBox _actionGroupBox;
        private MenuStrip _menuStrip;
        private ToolStripMenuItem _fileMenuItem;
        private ToolStripMenuItem _exportMenuItem;
        private ToolStripMenuItem _importMenuItem;
        private ToolStripMenuItem _helpMenuItem;
        private ToolStripMenuItem _aboutMenuItem;

        public MainForm()
        {
            _configManager = new ConfigManager();
            _proxyConfigManager = new ProxyConfigManager();
            _networkManager = new NetworkManager();
            _proxyManager = new ProxyManager();
            InitializeComponent();
            LoadAdapters();
            LoadConfigs();
            LoadProxyConfigs();
        }

        private void InitializeComponent()
        {
            Text = "IP配置管理器";
            Size = new Size(900, 700);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Sizable;
            MinimumSize = new Size(800, 600);

            // 创建菜单栏
            _menuStrip = new MenuStrip();
            _fileMenuItem = new ToolStripMenuItem("文件(&F)");
            _exportMenuItem = new ToolStripMenuItem("导出配置(&E)");
            _importMenuItem = new ToolStripMenuItem("导入配置(&I)");
            _helpMenuItem = new ToolStripMenuItem("帮助(&H)");
            _aboutMenuItem = new ToolStripMenuItem("关于(&A)");

            _fileMenuItem.DropDownItems.AddRange(new ToolStripItem[] { _exportMenuItem, _importMenuItem });
            _helpMenuItem.DropDownItems.Add(_aboutMenuItem);
            _menuStrip.Items.AddRange(new ToolStripItem[] { _fileMenuItem, _helpMenuItem });

            // 网络适配器选择
            _adapterLabel = new Label
            {
                Text = "网络适配器:",
                Location = new Point(15, 40),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _adapterComboBox = new ComboBox
            {
                Location = new Point(100, 40),
                Size = new Size(300, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            _refreshButton = new Button
            {
                Text = "刷新",
                Location = new Point(410, 40),
                Size = new Size(75, 23)
            };

            _getCurrentButton = new Button
            {
                Text = "获取当前配置",
                Location = new Point(495, 40),
                Size = new Size(100, 23)
            };

            _getProxyButton = new Button
            {
                Text = "获取代理配置",
                Location = new Point(605, 40),
                Size = new Size(100, 23)
            };

            // 网络IP配置组
            _configGroupBox = new GroupBox
            {
                Text = "网络IP配置",
                Location = new Point(15, 80),
                Size = new Size(420, 550)
            };

            _configListBox = new ListBox
            {
                Location = new Point(15, 25),
                Size = new Size(390, 450),
                DisplayMember = "Name"
            };

            _addButton = new Button
            {
                Text = "添加",
                Location = new Point(15, 485),
                Size = new Size(75, 30)
            };

            _editButton = new Button
            {
                Text = "编辑",
                Location = new Point(100, 485),
                Size = new Size(75, 30)
            };

            _deleteButton = new Button
            {
                Text = "删除",
                Location = new Point(185, 485),
                Size = new Size(75, 30)
            };

            _applyButton = new Button
            {
                Text = "应用",
                Location = new Point(270, 485),
                Size = new Size(75, 30)
            };

            _configGroupBox.Controls.AddRange(new Control[]
            {
                _configListBox, _addButton, _editButton, _deleteButton, _applyButton
            });

            // 代理服务器配置组
            _proxyConfigGroupBox = new GroupBox
            {
                Text = "代理服务器配置",
                Location = new Point(450, 80),
                Size = new Size(420, 550)
            };

            _proxyConfigListBox = new ListBox
            {
                Location = new Point(15, 25),
                Size = new Size(390, 450),
                DisplayMember = "Name"
            };

            _addProxyButton = new Button
            {
                Text = "添加",
                Location = new Point(15, 485),
                Size = new Size(75, 30)
            };

            _editProxyButton = new Button
            {
                Text = "编辑",
                Location = new Point(100, 485),
                Size = new Size(75, 30)
            };

            _deleteProxyButton = new Button
            {
                Text = "删除",
                Location = new Point(185, 485),
                Size = new Size(75, 30)
            };

            _applyProxyButton = new Button
            {
                Text = "应用",
                Location = new Point(270, 485),
                Size = new Size(75, 30)
            };

            _proxyConfigGroupBox.Controls.AddRange(new Control[]
            {
                _proxyConfigListBox, _addProxyButton, _editProxyButton, _deleteProxyButton, _applyProxyButton
            });

            // 操作组
            _actionGroupBox = new GroupBox
            {
                Text = "操作",
                Location = new Point(15, 640),
                Size = new Size(855, 50)
            };

            // 添加控件到窗体
            Controls.AddRange(new Control[]
            {
                _menuStrip, _adapterLabel, _adapterComboBox, _refreshButton, _getCurrentButton, _getProxyButton,
                _configGroupBox, _proxyConfigGroupBox, _actionGroupBox
            });

            // 绑定事件
            _refreshButton.Click += RefreshButton_Click;
            _getCurrentButton.Click += GetCurrentButton_Click;
            _getProxyButton.Click += GetProxyButton_Click;
            _addButton.Click += AddButton_Click;
            _editButton.Click += EditButton_Click;
            _deleteButton.Click += DeleteButton_Click;
            _applyButton.Click += ApplyButton_Click;
            _addProxyButton.Click += AddProxyButton_Click;
            _editProxyButton.Click += EditProxyButton_Click;
            _deleteProxyButton.Click += DeleteProxyButton_Click;
            _applyProxyButton.Click += ApplyProxyButton_Click;
            _adapterComboBox.SelectedIndexChanged += AdapterComboBox_SelectedIndexChanged;
            _configListBox.SelectedIndexChanged += ConfigListBox_SelectedIndexChanged;
            _proxyConfigListBox.SelectedIndexChanged += ProxyConfigListBox_SelectedIndexChanged;
            _exportMenuItem.Click += ExportMenuItem_Click;
            _importMenuItem.Click += ImportMenuItem_Click;
            _aboutMenuItem.Click += AboutMenuItem_Click;

            MainMenuStrip = _menuStrip;
            LoadAdapters();
            LoadConfigs();
        }

        private void LoadAdapters()
        {
            try
            {
                var adapters = _networkManager.GetNetworkAdapters();
                _adapterComboBox.DataSource = adapters;
                _adapterComboBox.DisplayMember = "Name";
                _adapterComboBox.ValueMember = "Id";

                if (adapters.Any())
                {
                    _adapterComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载网络适配器失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadConfigs()
        {
            try
            {
                var configs = _configManager.LoadConfigs();
                _configListBox.DataSource = configs;
                UpdateButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProxyConfigs()
        {
            try
            {
                var configs = _proxyConfigManager.LoadConfigs();
                _proxyConfigListBox.DataSource = configs;
                UpdateProxyButtonStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载代理配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadConfigsForCurrentAdapter()
        {
            if (_adapterComboBox.SelectedItem is NetworkAdapter adapter)
            {
                try
                {
                    var configs = _configManager.LoadConfigs()
                        .Where(c => c.AdapterId == adapter.Id || string.IsNullOrEmpty(c.AdapterId))
                        .ToList();
                    _configListBox.DataSource = configs;
                    UpdateButtonStates();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"加载配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateButtonStates()
        {
            var hasSelection = _configListBox.SelectedItem != null;
            var hasAdapter = _adapterComboBox.SelectedItem != null;

            _editButton.Enabled = hasSelection;
            _deleteButton.Enabled = hasSelection;
            _applyButton.Enabled = hasSelection && hasAdapter;
            _getCurrentButton.Enabled = hasAdapter;
        }

        private void UpdateProxyButtonStates()
        {
            var hasSelection = _proxyConfigListBox.SelectedItem != null;

            _editProxyButton.Enabled = hasSelection;
            _deleteProxyButton.Enabled = hasSelection;
            _applyProxyButton.Enabled = hasSelection;
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadAdapters();
        }

        private void GetCurrentButton_Click(object sender, EventArgs e)
        {
            if (_adapterComboBox.SelectedItem is not NetworkAdapter adapter)
            {
                MessageBox.Show("请选择网络适配器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var config = _networkManager.GetCurrentConfig(adapter.Id);
                if (config != null)
                {
                    using var form = new ConfigForm(config);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _configManager.AddConfig(form.NetworkConfig);
                        LoadConfigs();
                        MessageBox.Show("当前配置已保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取当前配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetProxyButton_Click(object sender, EventArgs e)
        {
            try
            {
                var config = _proxyManager.GetCurrentProxyConfig();
                if (config != null)
                {
                    using var form = new ProxyConfigForm(config);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        _proxyConfigManager.AddConfig(form.ProxyConfig);
                        LoadProxyConfigs();
                        MessageBox.Show("当前代理配置已保存", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取当前代理配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var adapter = _adapterComboBox.SelectedItem as NetworkAdapter;
            using var form = new ConfigForm();
            if (adapter != null)
            {
                form.NetworkConfig.AdapterId = adapter.Id;
                form.NetworkConfig.AdapterName = adapter.Name;
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                _configManager.AddConfig(form.NetworkConfig);
                LoadConfigs();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            if (_configListBox.SelectedItem is not NetworkConfig config)
            {
                return;
            }

            using var form = new ConfigForm(config);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _configManager.UpdateConfig(form.NetworkConfig);
                LoadConfigs();
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (_configListBox.SelectedItem is not NetworkConfig config)
            {
                return;
            }

            var result = MessageBox.Show($"确定要删除配置 '{config.Name}' 吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _configManager.DeleteConfig(config.Id);
                LoadConfigs();
            }
        }

        private async void ApplyButton_Click(object sender, EventArgs e)
        {
            if (_configListBox.SelectedItem is not NetworkConfig config)
            {
                return;
            }

            if (_adapterComboBox.SelectedItem is not NetworkAdapter adapter)
            {
                MessageBox.Show("请选择网络适配器", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                _applyButton.Enabled = false;
                _applyButton.Text = "应用中...";

                await _networkManager.ApplyConfigAsync(adapter.Id, config);
                MessageBox.Show($"配置 '{config.Name}' 已成功应用到 '{adapter.Name}'", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _applyButton.Enabled = true;
                _applyButton.Text = "应用";
            }
        }

        private void AdapterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
            LoadConfigsForCurrentAdapter();
        }

        private void ConfigListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateButtonStates();
        }

        private void ProxyConfigListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateProxyButtonStates();
        }

        private void ExportMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog
            {
                Filter = "JSON文件|*.json",
                Title = "导出配置",
                FileName = $"network_configs_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _configManager.ExportConfigs(dialog.FileName);
                    MessageBox.Show("配置导出成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导出配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ImportMenuItem_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Filter = "JSON文件|*.json",
                Title = "导入配置"
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    _configManager.ImportConfigs(dialog.FileName);
                    LoadConfigs();
                    MessageBox.Show("配置导入成功", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导入配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("IP配置管理器 v1.0\n\n一个用于管理Windows网络配置和代理设置的桌面应用程序。\n\n开发者: Assistant",
                "关于", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddProxyButton_Click(object sender, EventArgs e)
        {
            using var form = new ProxyConfigForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _proxyConfigManager.AddConfig(form.ProxyConfig);
                LoadProxyConfigs();
            }
        }

        private void EditProxyButton_Click(object sender, EventArgs e)
        {
            if (_proxyConfigListBox.SelectedItem is not ProxyConfig config)
            {
                return;
            }

            using var form = new ProxyConfigForm(config);
            if (form.ShowDialog() == DialogResult.OK)
            {
                _proxyConfigManager.UpdateConfig(form.ProxyConfig);
                LoadProxyConfigs();
            }
        }

        private void DeleteProxyButton_Click(object sender, EventArgs e)
        {
            if (_proxyConfigListBox.SelectedItem is not ProxyConfig config)
            {
                return;
            }

            var result = MessageBox.Show($"确定要删除代理配置 '{config.Name}' 吗？", "确认删除",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _proxyConfigManager.DeleteConfig(config.Id);
                LoadProxyConfigs();
            }
        }

        private async void ApplyProxyButton_Click(object sender, EventArgs e)
        {
            if (_proxyConfigListBox.SelectedItem is not ProxyConfig config)
            {
                return;
            }

            try
            {
                _applyProxyButton.Enabled = false;
                _applyProxyButton.Text = "应用中...";

                await _proxyManager.ApplyProxyConfigAsync(config);
                MessageBox.Show($"代理配置 '{config.Name}' 已成功应用", "成功",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"应用代理配置失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                _applyProxyButton.Enabled = true;
                _applyProxyButton.Text = "应用";
            }
        }
    }
}