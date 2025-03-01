using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;

namespace QuickConvert.App
{
    public static class RegistryHelper
    {
        private const string ContextMenuName = "QuickConvert";
        private const string ContextMenuText = "Convert with QuickConvert";

        /// <summary>
        /// Registers the application in the Windows Registry to add a context menu option
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        public static bool RegisterContextMenu()
        {
            try
            {
                string executablePath = Application.ExecutablePath;
                
                // Create a key for all file types
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\*\shell\QuickConvert"))
                {
                    key.SetValue("", ContextMenuText);
                    key.SetValue("Icon", executablePath);
                }

                // Create the command key
                using (RegistryKey key = Registry.CurrentUser.CreateSubKey(@"Software\Classes\*\shell\QuickConvert\command"))
                {
                    key.SetValue("", $"\"{executablePath}\" \"%1\"");
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to register context menu: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Unregisters the application from the Windows Registry
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        public static bool UnregisterContextMenu()
        {
            try
            {
                // Delete the context menu keys
                Registry.CurrentUser.DeleteSubKeyTree(@"Software\Classes\*\shell\QuickConvert", false);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to unregister context menu: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// Checks if the application is registered in the Windows Registry
        /// </summary>
        /// <returns>True if registered, false otherwise</returns>
        public static bool IsRegistered()
        {
            try
            {
                using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Classes\*\shell\QuickConvert"))
                {
                    return key != null;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Ensures the application is running with administrator privileges
        /// </summary>
        /// <returns>True if running as admin, false otherwise</returns>
        public static bool EnsureAdminPrivileges()
        {
            // Check if the current process has admin privileges
            bool isAdmin = new System.Security.Principal.WindowsPrincipal(
                System.Security.Principal.WindowsIdentity.GetCurrent())
                .IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

            if (!isAdmin)
            {
                // Restart the application with admin privileges
                try
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = Application.ExecutablePath;
                    startInfo.Verb = "runas"; // Request admin privileges
                    
                    // Pass all command-line arguments
                    startInfo.Arguments = string.Join(" ", Environment.GetCommandLineArgs().Skip(1)
                        .Select(arg => $"\"{arg}\""));
                    
                    Process.Start(startInfo);
                    return true;
                }
                catch
                {
                    MessageBox.Show("This operation requires administrator privileges.", "Admin Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }
    }
} 