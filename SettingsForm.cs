using System;
using System.Windows.Forms;

namespace QuickConvert.App
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            
            // Apply dark theme to this form
            ThemeManager.ApplyTheme(this);
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // Check if the application is registered in the context menu
            chkContextMenu.Checked = RegistryHelper.IsRegistered();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isRegistered = RegistryHelper.IsRegistered();
            
            if (chkContextMenu.Checked && !isRegistered)
            {
                // Need admin privileges to register
                if (RegistryHelper.EnsureAdminPrivileges())
                {
                    if (RegistryHelper.RegisterContextMenu())
                    {
                        MessageBox.Show("QuickConvert has been added to your context menu.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else if (!chkContextMenu.Checked && isRegistered)
            {
                // Need admin privileges to unregister
                if (RegistryHelper.EnsureAdminPrivileges())
                {
                    if (RegistryHelper.UnregisterContextMenu())
                    {
                        MessageBox.Show("QuickConvert has been removed from your context menu.", "Success",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
} 