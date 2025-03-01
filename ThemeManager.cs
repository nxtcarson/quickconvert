using System;
using System.Windows.Forms;

namespace QuickConvert.App
{
    public static class ThemeManager
    {
        // Dark theme colors (solid black with some contrast)
        public static readonly System.Drawing.Color DarkBackColor = System.Drawing.Color.FromArgb(10, 10, 10);
        public static readonly System.Drawing.Color DarkForeColor = System.Drawing.Color.FromArgb(240, 240, 240);
        public static readonly System.Drawing.Color DarkControlBackColor = System.Drawing.Color.FromArgb(25, 25, 25);
        public static readonly System.Drawing.Color DarkBorderColor = System.Drawing.Color.FromArgb(50, 50, 50);
        
        /// <summary>
        /// Applies the dark theme to a form
        /// </summary>
        /// <param name="form">The form to apply the theme to</param>
        public static void ApplyTheme(Form form)
        {
            // Round the form corners if needed and possible
            try
            {
                // Add rounded corners to the form
                form.FormBorderStyle = FormBorderStyle.FixedSingle;
                
                // Add shadow effect using form properties
                form.ShowInTaskbar = true;
            }
            catch
            {
                // If custom styling fails, continue without it
            }
            
            ApplyThemeToControl(form);
            
            // Recursively apply theme to all controls on the form
            ApplyThemeToControls(form.Controls);
        }
        
        /// <summary>
        /// Recursively applies the theme to a control collection
        /// </summary>
        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                ApplyThemeToControl(control);
                
                // Recursively apply to child controls
                if (control.Controls.Count > 0)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }
        
        /// <summary>
        /// Applies the theme to a specific control based on its type
        /// </summary>
        private static void ApplyThemeToControl(Control control)
        {
            // Default colors for all controls
            control.BackColor = DarkBackColor;
            control.ForeColor = DarkForeColor;
            
            // Special handling for specific control types
            if (control is Button button)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderColor = DarkBorderColor;
                button.BackColor = DarkControlBackColor;
                button.FlatAppearance.BorderSize = 1;
                button.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(70, 70, 70);
                button.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(45, 45, 45);
            }
            else if (control is TextBox textBox)
            {
                textBox.BorderStyle = BorderStyle.FixedSingle;
                textBox.BackColor = DarkControlBackColor;
            }
            else if (control is ListBox listBox)
            {
                listBox.BorderStyle = BorderStyle.FixedSingle;
                listBox.BackColor = DarkControlBackColor;
            }
            else if (control is ComboBox comboBox)
            {
                comboBox.FlatStyle = FlatStyle.Flat;
                comboBox.BackColor = DarkControlBackColor;
            }
            else if (control is CheckBox checkBox)
            {
                checkBox.FlatStyle = FlatStyle.Flat;
                checkBox.FlatAppearance.BorderColor = DarkBorderColor;
                checkBox.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(60, 60, 60);
            }
            else if (control is ProgressBar progressBar)
            {
                // Custom style for progress bar
                progressBar.BackColor = DarkControlBackColor;
                progressBar.ForeColor = System.Drawing.Color.FromArgb(70, 130, 180); // Steel blue for progress
            }
            // Add other control types as needed
        }
    }
} 