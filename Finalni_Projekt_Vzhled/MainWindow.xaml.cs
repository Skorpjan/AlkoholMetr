using cteniDatTest;
using System.IO;
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
using System.Xml.Serialization;

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
            LoadData();
            listboxData.ItemsSource = data.GetAll();

        }

        private void Continue1_Click(object sender, RoutedEventArgs e)
{
                Alkohol_detaily.Visibility = Visibility.Visible;
            
        }
        private void LoadData()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml");

            if (!File.Exists(path))
            {
                MessageBox.Show("data.xml not found!");
                data = new Database();  // prevent null refs
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(Database));
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                data = (Database)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data.xml: {ex.Message}");
                data = new Database();  // fallback
            }
        }

        private void Continue2_Click(object sender, RoutedEventArgs e)
        {
            if (CheckBoxMuz.IsChecked == false && CheckBoxZena.IsChecked == false)
            {
                MessageBoxResult result = MessageBox.Show("Není zaškrtlé žádné pohlaví!!!", "Výběr pohlaví", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(txtBoxWeight.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("Vyplňte svojí hmotnost!!!", "Zadání hmotnosti", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if(txtBoxDrinkStart.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("Vyplňte začátek pití ⊂(◉‿◉)つ", "Zadání začátku pití", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (txtBoxDrinkEnd.Text == "")
            {
                MessageBoxResult result = MessageBox.Show("Vyplňte konec pití(╥﹏╥)", "Zadání konce pití", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Vyber_alk.Visibility = Visibility.Visible;
            }
        }
    

        private void Gender_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == CheckBoxMuz && CheckBoxMuz.IsChecked == true)
                CheckBoxZena.IsChecked = false;
            else if (sender == CheckBoxZena && CheckBoxZena.IsChecked == true)
                CheckBoxMuz.IsChecked = false;
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        }
}