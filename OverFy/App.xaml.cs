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
        Work work;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
             work = new Work();

            work.Start();

        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            work.Stop();
        }
    }
}
