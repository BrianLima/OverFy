using System;
using System.Windows;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public SpotifyWorker work;
        public static AppSettings appSettings;
        public bool autoStarted = false;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            appSettings = new AppSettings();
            work = new SpotifyWorker();

            var startArg = Environment.GetCommandLineArgs();

            if (startArg != null)
            {
                foreach (var arg in startArg)
                {
                    if (arg.Contains("autostart"))
                    {
                        autoStarted = true;

                        break;
                    }
                }
            }

            work.Start();

            MainWindow window = new OverFy.MainWindow(work, appSettings);

            TrayIcon icon = new TrayIcon(window);
            //icon.Show();

            if (!autoStarted)
            {
                window.Show();
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            SpotifyWorker.ClearScreen();
            work.Stop();
            appSettings.Save();
        }
    }
}
