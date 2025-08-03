using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace IPConfiger
{
    /// <summary>
    /// 代理配置窗体
    /// </summary>
    public partial class ProxyConfigForm : Form
    {
        private GroupBox _proxyGroupBox;
        private CheckBox _useProxyCheckBox;
        private Label _proxyTypeLabel;
        private ComboBox _proxyTypeComboBox;
        private Label _proxyServerLabel;
        private TextBox _proxyServerTextBox;
        private Label _proxyPortLabel;
        private TextBox _proxyPortTextBox;
        private CheckBox _proxyAuthCheckBox;
        private Label _proxyUsernameLabel;
        private TextBox _proxyUsernameTextBox;
        private Label _proxyPasswordLabel;
        private TextBox _proxyPasswordTextBox;
        private Label _proxyBypassLabel;
        private TextBox _proxyBypassTextBox;
        private CheckBox _proxyBypassLocalCheckBox;
        private Label _nameLabel;
        private TextBox _nameTextBox;
        private Label _descriptionLabel;
        private TextBox _descriptionTextBox;
        private Button _okButton;
        private Button _cancelButton;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ProxyConfig ProxyConfig { get; set; } = new ProxyConfig();

        public ProxyConfigForm(ProxyConfig? config = null)
        {
            InitializeComponent();
            LoadConfig(config);
        }

        private void InitializeComponent()
        {
            Text = "代理服务器配置";
            Size = new Size(450, 600);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // 配置名称
            _nameLabel = new Label
            {
                Text = "配置名称:",
                Location = new Point(15, 20),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _nameTextBox = new TextBox
            {
                Location = new Point(100, 20),
                Size = new Size(320, 23)
            };

            // 配置描述
            _descriptionLabel = new Label
            {
                Text = "配置描述:",
                Location = new Point(15, 55),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _descriptionTextBox = new TextBox
            {
                Location = new Point(100, 55),
                Size = new Size(320, 23)
            };

            // 代理配置组
            _proxyGroupBox = new GroupBox
            {
                Text = "代理服务器设置",
                Location = new Point(15, 90),
                Size = new Size(405, 420)
            };

            // 启用代理
            _useProxyCheckBox = new CheckBox
            {
                Text = "启用代理服务器",
                Location = new Point(15, 25),
                Size = new Size(150, 23)
            };

            // 代理类型
            _proxyTypeLabel = new Label
            {
                Text = "代理类型:",
                Location = new Point(15, 60),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _proxyTypeComboBox = new ComboBox
            {
                Location = new Point(100, 60),
                Size = new Size(120, 23),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            _proxyTypeComboBox.Items.AddRange(new object[] { "HTTP", "HTTPS", "SOCKS4", "SOCKS5" });
            _proxyTypeComboBox.SelectedIndex = 0;

            // 代理服务器地址
            _proxyServerLabel = new Label
            {
                Text = "服务器地址:",
                Location = new Point(15, 95),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _proxyServerTextBox = new TextBox
            {
                Location = new Point(100, 95),
                Size = new Size(280, 23)
            };

            // 代理端口
            _proxyPortLabel = new Label
            {
                Text = "端口:",
                Location = new Point(15, 130),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _proxyPortTextBox = new TextBox
            {
                Location = new Point(100, 130),
                Size = new Size(100, 23),
                Text = "8080"
            };

            // 需要身份验证
            _proxyAuthCheckBox = new CheckBox
            {
                Text = "代理服务器需要身份验证",
                Location = new Point(15, 165),
                Size = new Size(200, 23)
            };

            // 用户名
            _proxyUsernameLabel = new Label
            {
                Text = "用户名:",
                Location = new Point(15, 200),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _proxyUsernameTextBox = new TextBox
            {
                Location = new Point(100, 200),
                Size = new Size(280, 23)
            };

            // 密码
            _proxyPasswordLabel = new Label
            {
                Text = "密码:",
                Location = new Point(15, 235),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _proxyPasswordTextBox = new TextBox
            {
                Location = new Point(100, 235),
                Size = new Size(280, 23),
                UseSystemPasswordChar = true
            };

            // 绕过代理的地址
            _proxyBypassLabel = new Label
            {
                Text = "绕过代理的地址:",
                Location = new Point(15, 270),
                Size = new Size(120, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _proxyBypassTextBox = new TextBox
            {
                Location = new Point(15, 295),
                Size = new Size(365, 60),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            // 绕过本地地址
            _proxyBypassLocalCheckBox = new CheckBox
            {
                Text = "对于本地地址不使用代理服务器",
                Location = new Point(15, 365),
                Size = new Size(250, 23),
                Checked = true
            };

            // 添加控件到代理组
            _proxyGroupBox.Controls.AddRange(new Control[]
            {
                _useProxyCheckBox, _proxyTypeLabel, _proxyTypeComboBox,
                _proxyServerLabel, _proxyServerTextBox, _proxyPortLabel, _proxyPortTextBox,
                _proxyAuthCheckBox, _proxyUsernameLabel, _proxyUsernameTextBox,
                _proxyPasswordLabel, _proxyPasswordTextBox, _proxyBypassLabel, _proxyBypassTextBox,
                _proxyBypassLocalCheckBox
            });

            // 确定按钮
            _okButton = new Button
            {
                Text = "确定",
                Location = new Point(265, 530),
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK
            };

            // 取消按钮
            _cancelButton = new Button
            {
                Text = "取消",
                Location = new Point(350, 530),
                Size = new Size(75, 30),
                DialogResult = DialogResult.Cancel
            };

            // 添加控件到窗体
            Controls.AddRange(new Control[]
            {
                _nameLabel, _nameTextBox, _descriptionLabel, _descriptionTextBox,
                _proxyGroupBox, _okButton, _cancelButton
            });

            // 绑定事件
            _useProxyCheckBox.CheckedChanged += UseProxyCheckBox_CheckedChanged;
            _proxyAuthCheckBox.CheckedChanged += ProxyAuthCheckBox_CheckedChanged;
            _okButton.Click += OkButton_Click;

            // 初始化控件状态
            UpdateProxyControlsState();
            UpdateProxyAuthControlsState();
        }

        private void LoadConfig(ProxyConfig? config)
        {
            if (config != null)
            {
                _nameTextBox.Text = config.Name;
                _descriptionTextBox.Text = config.Description;
                _useProxyCheckBox.Checked = config.UseProxy;
                _proxyTypeComboBox.SelectedIndex = (int)config.ProxyType;
                _proxyServerTextBox.Text = config.ProxyServer;
                _proxyPortTextBox.Text = config.ProxyPort.ToString();
                _proxyAuthCheckBox.Checked = config.ProxyRequiresAuth;
                _proxyUsernameTextBox.Text = config.ProxyUsername;
                _proxyPasswordTextBox.Text = config.ProxyPassword;
                _proxyBypassTextBox.Text = config.ProxyBypassList;
                _proxyBypassLocalCheckBox.Checked = config.ProxyBypassLocal;
            }
            else
            {
                _nameTextBox.Text = $"代理配置_{DateTime.Now:yyyyMMdd_HHmmss}";
                _useProxyCheckBox.Checked = false;
                _proxyTypeComboBox.SelectedIndex = 0;
                _proxyPortTextBox.Text = "8080";
                _proxyBypassLocalCheckBox.Checked = true;
            }

            UpdateProxyControlsState();
            UpdateProxyAuthControlsState();
        }

        private void UseProxyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateProxyControlsState();
        }

        private void ProxyAuthCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateProxyAuthControlsState();
        }

        private void UpdateProxyControlsState()
        {
            var enabled = _useProxyCheckBox.Checked;
            _proxyTypeLabel.Enabled = enabled;
            _proxyTypeComboBox.Enabled = enabled;
            _proxyServerLabel.Enabled = enabled;
            _proxyServerTextBox.Enabled = enabled;
            _proxyPortLabel.Enabled = enabled;
            _proxyPortTextBox.Enabled = enabled;
            _proxyAuthCheckBox.Enabled = enabled;
            _proxyBypassLabel.Enabled = enabled;
            _proxyBypassTextBox.Enabled = enabled;
            _proxyBypassLocalCheckBox.Enabled = enabled;

            UpdateProxyAuthControlsState();
        }

        private void UpdateProxyAuthControlsState()
        {
            var enabled = _useProxyCheckBox.Checked && _proxyAuthCheckBox.Checked;
            _proxyUsernameLabel.Enabled = enabled;
            _proxyUsernameTextBox.Enabled = enabled;
            _proxyPasswordLabel.Enabled = enabled;
            _proxyPasswordTextBox.Enabled = enabled;
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            // 创建代理配置对象
            ProxyConfig = new ProxyConfig
            {
                Name = _nameTextBox.Text.Trim(),
                Description = _descriptionTextBox.Text.Trim(),
                UseProxy = _useProxyCheckBox.Checked,
                ProxyType = (ProxyType)_proxyTypeComboBox.SelectedIndex,
                ProxyServer = _proxyServerTextBox.Text.Trim(),
                ProxyPort = int.TryParse(_proxyPortTextBox.Text, out int port) ? port : 8080,
                ProxyRequiresAuth = _proxyAuthCheckBox.Checked,
                ProxyUsername = _proxyUsernameTextBox.Text.Trim(),
                ProxyPassword = _proxyPasswordTextBox.Text,
                ProxyBypassList = _proxyBypassTextBox.Text.Trim(),
                ProxyBypassLocal = _proxyBypassLocalCheckBox.Checked
            };

            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateInput()
        {
            // 验证配置名称
            if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
            {
                MessageBox.Show("请输入配置名称", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _nameTextBox.Focus();
                return false;
            }

            // 如果启用代理，验证代理设置
            if (_useProxyCheckBox.Checked)
            {
                if (string.IsNullOrWhiteSpace(_proxyServerTextBox.Text))
                {
                    MessageBox.Show("请输入代理服务器地址", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _proxyServerTextBox.Focus();
                    return false;
                }

                if (!int.TryParse(_proxyPortTextBox.Text, out int port) || port < 1 || port > 65535)
                {
                    MessageBox.Show("请输入有效的端口号 (1-65535)", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _proxyPortTextBox.Focus();
                    return false;
                }

                if (_proxyAuthCheckBox.Checked && string.IsNullOrWhiteSpace(_proxyUsernameTextBox.Text))
                {
                    MessageBox.Show("启用身份验证时，请输入用户名", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _proxyUsernameTextBox.Focus();
                    return false;
                }
            }

            return true;
        }
    }
}