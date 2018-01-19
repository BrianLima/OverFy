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
            if (comboProperty.SelectedIndex == comboProperty.Items.Count -1)
            {
                result = CustomValue.Text;
            }
            else
            {
                result = comboProperty.Text;
            }

            DialogHost.CloseDialogCommand.Execute(result, this);
        }
    }
}
