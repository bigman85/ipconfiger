using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace IPConfiger
{
    /// <summary>
    /// 网络配置窗体
    /// </summary>
    public partial class ConfigForm : Form
    {
        private Label _nameLabel;
        private TextBox _nameTextBox;
        private Label _descriptionLabel;
        private TextBox _descriptionTextBox;
        private GroupBox _ipConfigGroupBox;
        private RadioButton _dhcpRadioButton;
        private RadioButton _staticRadioButton;
        private Label _ipLabel;
        private TextBox _ipTextBox;
        private Label _subnetLabel;
        private TextBox _subnetTextBox;
        private Label _gatewayLabel;
        private TextBox _gatewayTextBox;
        private Label _dns1Label;
        private TextBox _dns1TextBox;
        private Label _dns2Label;
        private TextBox _dns2TextBox;
        private Button _okButton;
        private Button _cancelButton;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public NetworkConfig NetworkConfig { get; set; } = new NetworkConfig();

        public ConfigForm(NetworkConfig? config = null)
        {
            InitializeComponent();
            LoadConfig(config);
        }

        private void InitializeComponent()
        {
            Text = "网络配置";
            Size = new Size(450, 500);
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

            // IP配置组
            _ipConfigGroupBox = new GroupBox
            {
                Text = "IP配置",
                Location = new Point(15, 90),
                Size = new Size(405, 320)
            };

            // DHCP选项
            _dhcpRadioButton = new RadioButton
            {
                Text = "自动获取IP地址(DHCP)",
                Location = new Point(15, 25),
                Size = new Size(200, 23),
                Checked = true
            };

            // 静态IP选项
            _staticRadioButton = new RadioButton
            {
                Text = "使用下面的IP地址",
                Location = new Point(15, 55),
                Size = new Size(200, 23)
            };

            // IP地址
            _ipLabel = new Label
            {
                Text = "IP地址:",
                Location = new Point(30, 90),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _ipTextBox = new TextBox
            {
                Location = new Point(115, 90),
                Size = new Size(150, 23)
            };

            // 子网掩码
            _subnetLabel = new Label
            {
                Text = "子网掩码:",
                Location = new Point(30, 125),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _subnetTextBox = new TextBox
            {
                Location = new Point(115, 125),
                Size = new Size(150, 23)
            };

            // 默认网关
            _gatewayLabel = new Label
            {
                Text = "默认网关:",
                Location = new Point(30, 160),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _gatewayTextBox = new TextBox
            {
                Location = new Point(115, 160),
                Size = new Size(150, 23)
            };

            // 首选DNS
            _dns1Label = new Label
            {
                Text = "首选DNS:",
                Location = new Point(30, 195),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _dns1TextBox = new TextBox
            {
                Location = new Point(115, 195),
                Size = new Size(150, 23)
            };

            // 备用DNS
            _dns2Label = new Label
            {
                Text = "备用DNS:",
                Location = new Point(30, 230),
                Size = new Size(80, 23),
                TextAlign = ContentAlignment.MiddleLeft
            };

            _dns2TextBox = new TextBox
            {
                Location = new Point(115, 230),
                Size = new Size(150, 23)
            };

            // 添加控件到IP配置组
            _ipConfigGroupBox.Controls.AddRange(new Control[]
            {
                _dhcpRadioButton, _staticRadioButton,
                _ipLabel, _ipTextBox, _subnetLabel, _subnetTextBox,
                _gatewayLabel, _gatewayTextBox, _dns1Label, _dns1TextBox,
                _dns2Label, _dns2TextBox
            });

            // 确定按钮
            _okButton = new Button
            {
                Text = "确定",
                Location = new Point(265, 430),
                Size = new Size(75, 30),
                DialogResult = DialogResult.OK
            };

            // 取消按钮
            _cancelButton = new Button
            {
                Text = "取消",
                Location = new Point(350, 430),
                Size = new Size(75, 30),
                DialogResult = DialogResult.Cancel
            };

            // 添加控件到窗体
            Controls.AddRange(new Control[]
            {
                _nameLabel, _nameTextBox, _descriptionLabel, _descriptionTextBox,
                _ipConfigGroupBox, _okButton, _cancelButton
            });

            // 绑定事件
            _dhcpRadioButton.CheckedChanged += DhcpRadioButton_CheckedChanged;
            _staticRadioButton.CheckedChanged += StaticRadioButton_CheckedChanged;
            _nameTextBox.TextChanged += NameTextBox_TextChanged;
            _okButton.Click += OkButton_Click;

            // 初始化控件状态
            UpdateStaticControlsState();
            UpdateButtonState();
        }

        private void LoadConfig(NetworkConfig? config)
        {
            if (config != null)
            {
                _nameTextBox.Text = config.Name;
                _descriptionTextBox.Text = config.Description;
                _dhcpRadioButton.Checked = config.UseDHCP;
                _staticRadioButton.Checked = !config.UseDHCP;
                _ipTextBox.Text = config.IPAddress;
                _subnetTextBox.Text = config.SubnetMask;
                _gatewayTextBox.Text = config.Gateway;
                _dns1TextBox.Text = config.PrimaryDNS;
                _dns2TextBox.Text = config.SecondaryDNS;
            }
            else
            {
                _nameTextBox.Text = $"配置_{DateTime.Now:yyyyMMdd_HHmmss}";
                _dhcpRadioButton.Checked = true;
            }

            UpdateStaticControlsState();
        }

        private void DhcpRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStaticControlsState();
        }

        private void StaticRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateStaticControlsState();
        }

        private void NameTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateButtonState();
        }

        private void UpdateStaticControlsState()
        {
            var enabled = _staticRadioButton.Checked;
            _ipLabel.Enabled = enabled;
            _ipTextBox.Enabled = enabled;
            _subnetLabel.Enabled = enabled;
            _subnetTextBox.Enabled = enabled;
            _gatewayLabel.Enabled = enabled;
            _gatewayTextBox.Enabled = enabled;
            _dns1Label.Enabled = enabled;
            _dns1TextBox.Enabled = enabled;
            _dns2Label.Enabled = enabled;
            _dns2TextBox.Enabled = enabled;
        }

        private void UpdateButtonState()
        {
            _okButton.Enabled = !string.IsNullOrWhiteSpace(_nameTextBox.Text);
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }

            // 创建网络配置对象
            NetworkConfig = new NetworkConfig
            {
                Name = _nameTextBox.Text.Trim(),
                Description = _descriptionTextBox.Text.Trim(),
                UseDHCP = _dhcpRadioButton.Checked,
                IPAddress = _ipTextBox.Text.Trim(),
                SubnetMask = _subnetTextBox.Text.Trim(),
                Gateway = _gatewayTextBox.Text.Trim(),
                PrimaryDNS = _dns1TextBox.Text.Trim(),
                SecondaryDNS = _dns2TextBox.Text.Trim()
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

            // 如果选择静态IP，验证IP配置
            if (_staticRadioButton.Checked)
            {
                if (!ValidateIPAddress(_ipTextBox.Text, "IP地址"))
                {
                    _ipTextBox.Focus();
                    return false;
                }

                if (!ValidateIPAddress(_subnetTextBox.Text, "子网掩码"))
                {
                    _subnetTextBox.Focus();
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(_gatewayTextBox.Text) &&
                    !ValidateIPAddress(_gatewayTextBox.Text, "默认网关"))
                {
                    _gatewayTextBox.Focus();
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(_dns1TextBox.Text) &&
                    !ValidateIPAddress(_dns1TextBox.Text, "首选DNS"))
                {
                    _dns1TextBox.Focus();
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(_dns2TextBox.Text) &&
                    !ValidateIPAddress(_dns2TextBox.Text, "备用DNS"))
                {
                    _dns2TextBox.Focus();
                    return false;
                }

                // 验证IP地址和网关是否在同一子网
                if (!string.IsNullOrWhiteSpace(_gatewayTextBox.Text) &&
                    !IsInSameSubnet(_ipTextBox.Text, _gatewayTextBox.Text, _subnetTextBox.Text))
                {
                    MessageBox.Show("IP地址和默认网关不在同一子网中", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    _gatewayTextBox.Focus();
                    return false;
                }
            }

            return true;
        }

        private bool ValidateIPAddress(string ip, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show($"请输入{fieldName}", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IPAddress.TryParse(ip, out _))
            {
                MessageBox.Show($"{fieldName}格式不正确", "验证失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsInSameSubnet(string ip1, string ip2, string subnetMask)
        {
            try
            {
                var addr1 = IPAddress.Parse(ip1);
                var addr2 = IPAddress.Parse(ip2);
                var mask = IPAddress.Parse(subnetMask);

                var network1 = new IPAddress(addr1.GetAddressBytes().Zip(mask.GetAddressBytes(), (a, m) => (byte)(a & m)).ToArray());
                var network2 = new IPAddress(addr2.GetAddressBytes().Zip(mask.GetAddressBytes(), (a, m) => (byte)(a & m)).ToArray());

                return network1.Equals(network2);
            }
            catch
            {
                return false;
            }
        }
    }
}