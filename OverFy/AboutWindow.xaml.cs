using IWshRuntimeLibrary;
using System;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.IO;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        FileInfo localFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\" + "OverFy" + ".exe");
        FileInfo shortcutFile = new FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.Startup) + @"\OverFy.lnk");

        public AboutWindow()
        {
            InitializeComponent();

            if (shortcutFile.Exists)
            {
                autostart_toggle.IsChecked = true;
            }
        }

        private void Chip_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://twitter.com/brianostorm");
        }

        private void Chip1_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/brianlima");
        }

        private void Chip2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=9YPV3FHEFRAUQ");
        }

        private void Chip3_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://blockchain.info/pt/address/174LnSVCdrX4CnVS84jom7by2hMBGvJobm");
        }

        private void SetAutoStart()
        {
            try
            {
                WshShell shell = new WshShell();
                WshShortcut shortcut = (WshShortcut)shell.CreateShortcut(shortcutFile.FullName);
                shortcut.TargetPath = localFile.FullName;
                shortcut.IconLocation = localFile.FullName + ",0";
                shortcut.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
                shortcut.Arguments = "autostart";
                shortcut.Save();
            }
            catch (Exception e)
            {
                throw new Exception("Error setting to auto start" + Environment.NewLine + e.Message);
            }

        }

        private void autostart_toggle_Checked(object sender, RoutedEventArgs e)
        {
            if (!shortcutFile.Exists)
            {
                SetAutoStart();
            }
        }

        private void autostart_toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            if (shortcutFile.Exists)
            {
                localFile.Delete();
            }
        }

        private void Chip_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}