using MaterialDesignThemes.Wpf;
using System;
using System.Windows;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Keys keys;
        public SpotifyWorker worker;
        public static AppSettings appSettings;
        public bool autoStarted = false;
        private static PaletteHelper paletteHelper;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            appSettings = new AppSettings();
            keys = Keys.LoadKeys();

            if (keys.u == null)
            {
                AuthorizeSpotify w = new AuthorizeSpotify();
                w.ShowDialog();
            }

            worker = new SpotifyWorker();
            paletteHelper = new PaletteHelper();

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

            worker.Start();

            MainWindow window = new MainWindow(worker, appSettings);
            TrayIcon icon = new TrayIcon(window);

            SetLightDarkMode();

            if (!autoStarted)
            {
                window.Show();
            }
        }

        public static void SetLightDarkMode()
        {
            App.paletteHelper.SetLightDark(App.appSettings.DarkMode);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            worker.Stop();

            SpotifyWorker.ClearScreen();
            appSettings.Save();
        }
    }
}
