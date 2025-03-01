using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.PixelFormats;

namespace QuickConvert.App
{
    public partial class MainForm : Form
    {
        private string? _sourceFilePath;
        private Dictionary<string, List<string>> _conversionMap = new();

        public MainForm(string[] args)
        {
            InitializeComponent();
            InitializeConversionMap();
            
            // Apply the dark theme to the form
            ThemeManager.ApplyTheme(this);

            // If file path is passed as an argument
            if (args.Length > 0 && File.Exists(args[0]))
            {
                _sourceFilePath = args[0];
                lblSelectedFile.Text = Path.GetFileName(_sourceFilePath);
                PopulateConversionOptions(_sourceFilePath);
            }
        }

        private void InitializeConversionMap()
        {
            _conversionMap = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                // Image formats
                { ".jpg", new List<string> { ".png", ".bmp", ".gif", ".tiff", ".webp" } },
                { ".jpeg", new List<string> { ".png", ".bmp", ".gif", ".tiff", ".webp" } },
                { ".png", new List<string> { ".jpg", ".bmp", ".gif", ".tiff", ".webp" } },
                { ".bmp", new List<string> { ".jpg", ".png", ".gif", ".tiff", ".webp" } },
                { ".gif", new List<string> { ".jpg", ".png", ".bmp", ".tiff" } },
                { ".tiff", new List<string> { ".jpg", ".png", ".bmp", ".gif" } },
                { ".webp", new List<string> { ".jpg", ".png", ".bmp", ".gif" } },
                
                // Document formats
                { ".txt", new List<string> { ".rtf", ".html", ".md" } },
                { ".rtf", new List<string> { ".txt", ".html" } },
                { ".md", new List<string> { ".txt", ".html" } },
                
                // Audio formats
                { ".mp3", new List<string> { ".wav", ".ogg", ".flac" } },
                { ".wav", new List<string> { ".mp3", ".ogg", ".flac" } },
                { ".ogg", new List<string> { ".mp3", ".wav" } },
                { ".flac", new List<string> { ".mp3", ".wav" } },
                
                // Video formats
                { ".mp4", new List<string> { ".avi", ".mov", ".wmv" } },
                { ".avi", new List<string> { ".mp4", ".mov", ".wmv" } },
                { ".mov", new List<string> { ".mp4", ".avi", ".wmv" } },
                { ".wmv", new List<string> { ".mp4", ".avi", ".mov" } }
            };
        }

        private void PopulateConversionOptions(string filePath)
        {
            lstTargetFormats.Items.Clear();
            
            string extension = Path.GetExtension(filePath);
            
            if (_conversionMap.ContainsKey(extension))
            {
                foreach (var format in _conversionMap[extension])
                {
                    lstTargetFormats.Items.Add(format);
                }
            }
            
            if (lstTargetFormats.Items.Count > 0)
            {
                lstTargetFormats.SelectedIndex = 0;
                btnConvert.Enabled = true;
            }
            else
            {
                btnConvert.Enabled = false;
                MessageBox.Show("No conversion options available for this file type.", "QuickConvert", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    _sourceFilePath = openFileDialog.FileName;
                    lblSelectedFile.Text = Path.GetFileName(_sourceFilePath);
                    PopulateConversionOptions(_sourceFilePath);
                }
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_sourceFilePath) || lstTargetFormats.SelectedItem == null)
                return;

            string targetExtension = lstTargetFormats.SelectedItem.ToString() ?? string.Empty;
            string targetFilePath = Path.Combine(
                Path.GetDirectoryName(_sourceFilePath) ?? string.Empty,
                Path.GetFileNameWithoutExtension(_sourceFilePath) + targetExtension);

            // Confirm if file exists
            if (File.Exists(targetFilePath))
            {
                DialogResult result = MessageBox.Show(
                    $"The file {Path.GetFileName(targetFilePath)} already exists. Do you want to replace it?",
                    "File exists", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                    {
                        saveFileDialog.FileName = Path.GetFileName(targetFilePath);
                        saveFileDialog.InitialDirectory = Path.GetDirectoryName(targetFilePath);
                        saveFileDialog.Filter = $"All files (*.*)|*.*";

                        if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        {
                            targetFilePath = saveFileDialog.FileName;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }

            // Create and show progress form
            using (var progressForm = new ProgressForm("Converting file..."))
            {
                // Don't show the form yet - we'll use ShowDialog at the end
                
                // Use a background worker to prevent UI freezing
                using (var worker = new System.ComponentModel.BackgroundWorker())
                {
                    worker.WorkerReportsProgress = true;
                    worker.DoWork += (s, args) =>
                    {
                        try
                        {
                            // Report starting progress
                            worker.ReportProgress(0);
                            
                            // Perform the conversion
                            ConvertFile(_sourceFilePath, targetFilePath);
                            
                            // Report completion
                            worker.ReportProgress(100);
                            
                            // Store result for the RunWorkerCompleted event
                            args.Result = true;
                        }
                        catch (Exception ex)
                        {
                            args.Result = ex;
                        }
                    };
                    
                    worker.ProgressChanged += (s, args) =>
                    {
                        progressForm.UpdateProgress(args.ProgressPercentage);
                    };
                    
                    worker.RunWorkerCompleted += (s, args) =>
                    {
                        // Allow the dialog to close
                        progressForm.DialogResult = DialogResult.OK;
                        
                        if (args.Result is Exception ex)
                        {
                            MessageBox.Show($"Error during conversion: {ex.Message}", "Conversion Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (args.Result is bool success && success)
                        {
                            DialogResult openResult = MessageBox.Show(
                                "Conversion completed successfully. Do you want to open the converted file?",
                                "Conversion Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            
                            if (openResult == DialogResult.Yes)
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = targetFilePath,
                                    UseShellExecute = true
                                });
                            }
                        }
                    };
                    
                    // Start the background worker
                    worker.RunWorkerAsync();
                    
                    // Show the progress form as a modal dialog
                    progressForm.ShowDialog(this);
                }
            }
        }

        private void ConvertFile(string sourcePath, string targetPath)
        {
            string sourceExtension = Path.GetExtension(sourcePath).ToLower();
            string targetExtension = Path.GetExtension(targetPath).ToLower();

            // Handle image conversions
            if (IsImageFile(sourceExtension) && IsImageFile(targetExtension))
            {
                // Check if WebP is involved - if so, use ImageSharp
                if (sourceExtension == ".webp" || targetExtension == ".webp")
                {
                    ConvertImageWithImageSharp(sourcePath, targetPath);
                }
                else
                {
                    // Use System.Drawing for standard formats
                    ConvertImage(sourcePath, targetPath);
                }
                return;
            }

            // For other types, we'll need to implement specific converters
            // or use external libraries. For now, let's throw a not implemented exception
            // for anything we can't handle natively.
            throw new NotImplementedException($"Conversion from {sourceExtension} to {targetExtension} is not implemented yet.");
        }

        private bool IsImageFile(string extension)
        {
            return new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" }
                .Contains(extension.ToLower());
        }

        private void ConvertImage(string sourcePath, string targetPath)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(sourcePath))
            {
                string targetExtension = Path.GetExtension(targetPath).ToLower();
                ImageFormat format = GetImageFormat(targetExtension);
                
                image.Save(targetPath, format);
            }
        }

        private ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                case ".tiff":
                    return ImageFormat.Tiff;
                default:
                    return ImageFormat.Jpeg; // Default to JPEG
            }
        }

        private void ConvertImageWithImageSharp(string sourcePath, string targetPath)
        {
            string targetExtension = Path.GetExtension(targetPath).ToLower();
            
            try
            {
                // Use a simpler approach that works with the current ImageSharp version
                using (var image = SixLabors.ImageSharp.Image.Load(sourcePath))
                {
                    // Save with the appropriate encoder
                    switch (targetExtension)
                    {
                        case ".jpg":
                        case ".jpeg":
                            image.Save(targetPath, new JpegEncoder { Quality = 85 });
                            break;
                        case ".png":
                            image.Save(targetPath, new PngEncoder());
                            break;
                        case ".bmp":
                            image.Save(targetPath, new BmpEncoder());
                            break;
                        case ".gif":
                            image.Save(targetPath, new GifEncoder());
                            break;
                        case ".tiff":
                            image.Save(targetPath, new TiffEncoder());
                            break;
                        default:
                            // Default to PNG
                            image.Save(targetPath, new PngEncoder());
                            break;
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                // For very large images, provide more detailed error
                throw new Exception("Out of memory during conversion. The image is too large to process with available memory. " +
                                    "Try reducing the image size or freeing up system memory before converting.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during conversion: {ex.Message}");
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            using (SettingsForm settingsForm = new SettingsForm())
            {
                settingsForm.ShowDialog(this);
            }
        }
    }
}
