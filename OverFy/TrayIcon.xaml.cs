using System.Windows;

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

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
