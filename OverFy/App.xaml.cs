using System;
using System.Windows;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Work work;
        public static AppSettings appSettings;
        public bool autoStarted = false;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            appSettings = new AppSettings();
            work = new Work();

            var startArg = Environment.GetCommandLineArgs();

            if (startArg != null)
            {
                foreach (var arg in startArg)
                {
                    if (arg.Contains("autostart"))
                    {
                        autoStarted = true;
                        work.Start();
                        break;
                    }
                }
            }

            if (!autoStarted)
            {
                MainWindow window = new OverFy.MainWindow(work, appSettings);
                window.Show();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            work.Stop();
            appSettings.Save();
        }
    }
}
