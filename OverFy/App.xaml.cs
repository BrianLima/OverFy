using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Work work;
        public bool autoStarted = false;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
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
                MainWindow window = new OverFy.MainWindow(work);
                window.Show();
            }

        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            work.Stop();
        }

    }
}
