using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for TrayIcon.xaml
    /// </summary>
    public partial class TrayIcon : Window
    {
        MainWindow mainWindow;
        public TrayIcon(MainWindow w)
        {
            InitializeComponent();

            mainWindow = w;
        }

        private void icon_TrayMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
        }
    }
}
