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
            var path = "data.xml";
            if (!File.Exists(path))
            {
                MessageBox.Show("data.xml nenalezeno!"); //data.xml jsou uložena v C:\Users\[USER]\source\repos\[JMENOREPOSITARE]\[JMENOPROJEKTU]\bin\Debug\net8.0-windows
                data = new Database();
                return;
            }

            var serializer = new XmlSerializer(typeof(Database)); //vytvoří se XmlSerializer pro typ Database, otevře se FileStream a data se z XML načtou do proměnné data
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                data = (Database)serializer.Deserialize(fs) as Database;
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