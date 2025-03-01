using System;
using System.Windows.Forms;

namespace QuickConvert.App;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        // Increase the Large Object Heap limit to help with large image processing
        // This helps prevent out of memory errors with large WebP files
        GC.AddMemoryPressure(1024 * 1024 * 100); // Add 100MB pressure to allocate more memory
        
        // Set DPI mode
        Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
        
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        
        // Create and run the main form
        var mainForm = new MainForm(args);
        Application.Run(mainForm);
        
        // Release the memory pressure when the application exits
        GC.RemoveMemoryPressure(1024 * 1024 * 100);
    }    
}