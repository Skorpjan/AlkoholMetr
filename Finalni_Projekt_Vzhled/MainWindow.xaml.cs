using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Finalni_Projekt_Vzhled
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Continue1_Click(object sender, RoutedEventArgs e)
        {
            Alkohol_detaily.Visibility = Visibility.Visible;
        }

        private void Continue2_Click(object sender, RoutedEventArgs e)
        {
            Vyber_alk.Visibility = Visibility.Visible;
        }
    

        private void Gender_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == CheckBoxMuz && CheckBoxMuz.IsChecked == true)
                CheckBoxZena.IsChecked = false;
            else if (sender == CheckBoxZena && CheckBoxZena.IsChecked == true)
                CheckBoxMuz.IsChecked = false;
        }

        private void Gender_Unchecked(object sender, RoutedEventArgs e)
        {
            // Volitelně můžeš doplnit logiku
        }
    }
}