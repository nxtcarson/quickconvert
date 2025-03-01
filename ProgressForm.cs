using System;
using System.Windows.Forms;

namespace QuickConvert.App
{
    public partial class ProgressForm : Form
    {
        public ProgressForm(string title)
        {
            InitializeComponent();
            this.Text = title;
            
            // Apply dark theme to this form
            ThemeManager.ApplyTheme(this);
            
            // Prevent the form from being closed by the user
            this.ControlBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            
            // Initialize progress display
            progressBar.Minimum = 0;
            progressBar.Maximum = 100;
            progressBar.Value = 0;
        }
        
        public void UpdateProgress(int percentage)
        {
            // Ensure value is within valid range
            percentage = Math.Max(0, Math.Min(100, percentage));
            
            if (InvokeRequired)
            {
                Invoke(new Action<int>(UpdateProgress), percentage);
                return;
            }
 
            progressBar.Value = percentage;
            lblStatus.Text = $"Progress: {percentage}%";
            
            // Update status text when complete
            if (percentage == 100)
            {
                lblStatus.Text = "Complete!";
            }
            
            // Force UI update
            Application.DoEvents();
        }

        // Override OnFormClosing to prevent user from closing the dialog
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Only allow the form to close if DialogResult is set
            if (this.DialogResult != DialogResult.OK && 
                this.DialogResult != DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            
            base.OnFormClosing(e);
        }
    }
} 