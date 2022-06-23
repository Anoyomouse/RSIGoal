/**
 * Copyright (c) David-John Miller AKA Anoyomouse 2014
 *
 * See LICENCE in the project directory for licence information
 **/
using System;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace RSIGoal
{
    [SupportedOSPlatform("windows")]
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            #pragma warning disable CA1416 // Validate platform compatibility
            var mainForm = new frmMain();
            #pragma warning restore CA1416 // Validate platform compatibility
            Application.Run(mainForm);
        }
    }
}
