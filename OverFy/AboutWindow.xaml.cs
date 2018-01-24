using Microsoft.Win32.TaskScheduler;
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

        public AboutWindow()
        {
            InitializeComponent();

            autostart_toggle.DataContext = App.appSettings.AutoStart;
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
            //Administrator rights are only needed because RivaTuner it self requires administrator rights to be run.
            //By using a task, when autorunning the app the first time on user logon, we can check if riva itself
            //was autostarted too, and if not, force it, and this is not possible by simply using a shortcut on the startup folder
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Author = "Briano";
                td.RegistrationInfo.Description = "OverFy Auto Run on Windows Startup";
                td.Principal.RunLevel = TaskRunLevel.Highest;
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                td.Triggers.Add(new LogonTrigger() { });

                td.Actions.Add(new ExecAction(localFile.FullName, "autostart" ,null));

                ts.RootFolder.RegisterTaskDefinition(@"OverFy", td);

                App.appSettings.AutoStart = true;
            }
        }

        private void autostart_toggle_Checked(object sender, RoutedEventArgs e)
        {
            if (!App.appSettings.AutoStart)
            {
                SetAutoStart();
            }
        }

        private void autostart_toggle_Unchecked(object sender, RoutedEventArgs e)
        {
            using (TaskService ts = new TaskService())
            {
                //Remove the auto start task
                ts.RootFolder.DeleteTask("OverFy", false);
            }
        }

        private void Chip_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}