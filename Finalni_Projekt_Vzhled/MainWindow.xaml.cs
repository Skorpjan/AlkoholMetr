
using cteniDat;
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
        private  cteniDat.Database data;
        private List<Alcohol> allAlcohols = new();

        private bool isMale = false;    //defaultni hodnota pro pohlavi 


        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void Continue1_Click(object sender, RoutedEventArgs e)
        {
            Alkohol_detaily.Visibility = Visibility.Visible; // pouze zobrazeni dalsich elementu po zadani inputu
            if(string.IsNullOrWhiteSpace(txtBoxWeight.Text) || string.IsNullOrWhiteSpace(txtBoxDrinkStart.Text) || string.IsNullOrWhiteSpace(txtBoxDrinkEnd.Text))
            {
                MessageBoxResult result = MessageBox.Show("Vyplňte všechny údaje!!!", "Zadání údajů", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            continue1Button.Visibility = Visibility.Collapsed; // skryti tlacitka pro pokracovani
            Vyber_alk.Visibility = Visibility.Visible;

        } // pouze zobrazeni dalsich elementu po zadani inputu + kontorla spravnosti zadanych dat
        private void LoadData() // tahame data z data.xml, osetreni poked data.xml neexistuje
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data.xml"); //slozeni pathu do data.xml

            if (!File.Exists(path))
            {
                MessageBox.Show("data.xml not found!");
                data = new Database(); 
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(Database));
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                data = (Database)serializer.Deserialize(fs);
                allAlcohols = data.Alcohols.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data.xml: {ex.Message}");
                data = new Database(); 
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = FilterTextBox.Text.Trim().ToLower();

            var filtered = allAlcohols
                .Where(a => a.Name.ToLower().Contains(filter))
                .ToList();

            DrinkComboBox.ItemsSource = filtered;
        } //metoda pro filtraci alkoholu podle jmena

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
        } // pouze zobrazeni dalsich elementu po zadani inputu + kontorla spravnosti dat


        private void Gender_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == CheckBoxMuz && CheckBoxMuz.IsChecked == true)
                CheckBoxZena.IsChecked = false;
            else if (sender == CheckBoxZena && CheckBoxZena.IsChecked == true)
                CheckBoxMuz.IsChecked = false;
        }   // pouze zobrazeni dalsich elementu po zadani inputu + kontorla spravnosti dat

        private void DrinkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrinkComboBox.SelectedItem is Alcohol selectedAlcohol)
            {
                switch (selectedAlcohol.Type.ToLower())
                {
                    case "beer":
                        AlcButton1.Content = "0.3 l";
                        AlcButton2.Content = "0.5 l";
                        AlcButton3.Content = "1 l";
                        break;
                    case "distillate":
                        AlcButton1.Content = "0.02 l";
                        AlcButton2.Content = "0.05 l";
                        AlcButton3.Content = "0.1 l";
                        break;
                    case "vine":
                        AlcButton1.Content = "0.1 l";
                        AlcButton2.Content = "0.15 l";
                        AlcButton3.Content = "0.2 l";
                        break;
                    default:
                        AlcButton1.Content = "0.1 l";
                        AlcButton2.Content = "0.3 l";
                        AlcButton3.Content = "0.5 l";
                        break;
                }
            }
        }



    }
}