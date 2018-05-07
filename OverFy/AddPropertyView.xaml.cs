using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace OverFy
{
    /// <summary>
    /// Interaction logic for AddPropertyView.xaml
    /// </summary>
    public partial class AddPropertyView : UserControl
    {
        public AddPropertyView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string result = string.Empty;

            //Always use Custom Property as the last index
            if (comboProperty.SelectedIndex == 8)
            {
                result = tbxCustomValue.Text;
            }
            else if (comboProperty.SelectedIndex == 9)
            {
                result = "BTC/" + comboCurrency.Text; 
            }
            else
            {
                result = comboProperty.Text;
            }

            DialogHost.CloseDialogCommand.Execute(result, this);
        }

        private void comboProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex == 9)
            {
                comboCurrency.Visibility = Visibility.Visible;
                labelCurrency.Visibility = Visibility.Visible;
            }
            else
            {
                comboCurrency.Visibility = Visibility.Collapsed;
                labelCurrency.Visibility = Visibility.Collapsed;
            }

            if (((ComboBox)sender).SelectedIndex == 8)
            {
                tbxCustomValue.Visibility = Visibility.Visible;
                labelCustomValue.Visibility = Visibility.Visible;
            }
            else
            {
                tbxCustomValue.Visibility = Visibility.Collapsed;
                labelCustomValue.Visibility = Visibility.Collapsed;
            } 
        }
    }
}
