namespace QuickConvert.App;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.lblFilePrompt = new System.Windows.Forms.Label();
        this.lblSelectedFile = new System.Windows.Forms.Label();
        this.btnBrowse = new System.Windows.Forms.Button();
        this.lblTargetFormat = new System.Windows.Forms.Label();
        this.lstTargetFormats = new System.Windows.Forms.ListBox();
        this.btnConvert = new System.Windows.Forms.Button();
        this.btnSettings = new System.Windows.Forms.Button();
        this.SuspendLayout();
        // 
        // lblFilePrompt
        // 
        this.lblFilePrompt.AutoSize = true;
        this.lblFilePrompt.Location = new System.Drawing.Point(14, 14);
        this.lblFilePrompt.Name = "lblFilePrompt";
        this.lblFilePrompt.Size = new System.Drawing.Size(76, 15);
        this.lblFilePrompt.TabIndex = 0;
        this.lblFilePrompt.Text = "Selected File:";
        // 
        // lblSelectedFile
        // 
        this.lblSelectedFile.AutoSize = true;
        this.lblSelectedFile.Location = new System.Drawing.Point(96, 14);
        this.lblSelectedFile.Name = "lblSelectedFile";
        this.lblSelectedFile.Size = new System.Drawing.Size(87, 15);
        this.lblSelectedFile.TabIndex = 1;
        this.lblSelectedFile.Text = "No file selected";
        // 
        // btnBrowse
        // 
        this.btnBrowse.Location = new System.Drawing.Point(297, 10);
        this.btnBrowse.Name = "btnBrowse";
        this.btnBrowse.Size = new System.Drawing.Size(75, 23);
        this.btnBrowse.TabIndex = 2;
        this.btnBrowse.Text = "Browse...";
        this.btnBrowse.UseVisualStyleBackColor = true;
        this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
        // 
        // lblTargetFormat
        // 
        this.lblTargetFormat.AutoSize = true;
        this.lblTargetFormat.Location = new System.Drawing.Point(14, 47);
        this.lblTargetFormat.Name = "lblTargetFormat";
        this.lblTargetFormat.Size = new System.Drawing.Size(88, 15);
        this.lblTargetFormat.TabIndex = 3;
        this.lblTargetFormat.Text = "Target Format:";
        // 
        // lstTargetFormats
        // 
        this.lstTargetFormats.FormattingEnabled = true;
        this.lstTargetFormats.ItemHeight = 15;
        this.lstTargetFormats.Location = new System.Drawing.Point(14, 65);
        this.lstTargetFormats.Name = "lstTargetFormats";
        this.lstTargetFormats.Size = new System.Drawing.Size(358, 139);
        this.lstTargetFormats.TabIndex = 4;
        // 
        // btnConvert
        // 
        this.btnConvert.Enabled = false;
        this.btnConvert.Location = new System.Drawing.Point(297, 210);
        this.btnConvert.Name = "btnConvert";
        this.btnConvert.Size = new System.Drawing.Size(75, 23);
        this.btnConvert.TabIndex = 5;
        this.btnConvert.Text = "Convert";
        this.btnConvert.UseVisualStyleBackColor = true;
        this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
        // 
        // btnSettings
        // 
        this.btnSettings.Location = new System.Drawing.Point(14, 210);
        this.btnSettings.Name = "btnSettings";
        this.btnSettings.Size = new System.Drawing.Size(75, 23);
        this.btnSettings.TabIndex = 6;
        this.btnSettings.Text = "Settings";
        this.btnSettings.UseVisualStyleBackColor = true;
        this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(384, 241);
        this.Controls.Add(this.btnSettings);
        this.Controls.Add(this.btnConvert);
        this.Controls.Add(this.lstTargetFormats);
        this.Controls.Add(this.lblTargetFormat);
        this.Controls.Add(this.btnBrowse);
        this.Controls.Add(this.lblSelectedFile);
        this.Controls.Add(this.lblFilePrompt);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "QuickConvert";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Label lblFilePrompt;
    private System.Windows.Forms.Label lblSelectedFile;
    private System.Windows.Forms.Button btnBrowse;
    private System.Windows.Forms.Label lblTargetFormat;
    private System.Windows.Forms.ListBox lstTargetFormats;
    private System.Windows.Forms.Button btnConvert;
    private System.Windows.Forms.Button btnSettings;
}
