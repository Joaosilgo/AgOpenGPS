﻿using AgOpenGPS.Properties;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly Mutex Mutex = new Mutex(true, "{516-0AC5-B9A1-55fd-A8CE-72F04E6BDE8F}");

        [STAThread]
        private static void Main()
        {
            // Copy user settings from previous application version if necessary
            //if (Properties.Settings.Default.UpdateSettings)
            //{
            //    Properties.Settings.Default.Upgrade();
            //    Properties.Settings.Default.Reload();
            //    Properties.Settings.Default.UpdateSettings = false;
            //    Properties.Settings.Default.Save();
            //}

            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                //if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

                ////opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                ////create default keys if not existing
                if (regKey == null)
                {
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                    //storing the values
                    Key.SetValue("Language", "en");
                    Key.SetValue("Directory", "Default");
                    Key.Close();

                    Settings.Default.setF_culture = "en";
                    Settings.Default.setF_workingDirectory = "Default";
                    Settings.Default.Save();
                }
                else
                {
                    Settings.Default.setF_culture = regKey.GetValue("Language").ToString();
                    Settings.Default.setF_workingDirectory = regKey.GetValue("Directory").ToString();
                    Settings.Default.Save();
                    regKey.Close();
                }


                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Properties.Settings.Default.setF_culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.setF_culture);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormGPS());
            }
            else
            {
                MessageBox.Show("AgOpenGPS is Already Running");
            }

        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private static extern bool SetProcessDPIAware();
    }
}