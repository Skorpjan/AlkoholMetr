
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
        bool isKilogram = true;
        private cteniDat.Database data;
        private List<Alcohol> allAlcohols = new();

        private readonly Dictionary<string, List<double>> drinkVolumes = new()
        {
        { "Beer", new List<double> { 0.3, 0.5, 1.0 } },
        { "istillate", new List<double> { 0.02, 0.04, 0.05 } },
        { "Vine", new List<double> { 0.1, 0.2, 0.3 } },
        { "Default", new List<double> { 0.1, 0.3, 0.5 } }
        };
        private List<double> currentButtonVolumes = new() { 0.1, 0.3, 0.5 };

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        private void Continue2_Click(object sender, RoutedEventArgs e)
        {
            Alkohol_detaily.Visibility = Visibility.Visible; // pouze zobrazeni dalsich elementu po zadani inputu
            if (string.IsNullOrWhiteSpace(txtBoxWeight.Text) || string.IsNullOrWhiteSpace(txtBoxDrinkStart.Text) || string.IsNullOrWhiteSpace(txtBoxDrinkEnd.Text))
            {
                MessageBoxResult result = MessageBox.Show("Vyplňte všechny údaje!!!", "Zadání údajů", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(txtBoxWeight.Text, out int weight) || weight <= 0)
            {
                MessageBox.Show("Hmotnost musí být kladné celé číslo!", "Chybný vstup", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // Kontrola času
            if (!TimeSpan.TryParseExact(txtBoxDrinkStart.Text, @"hh\:mm", null, out TimeSpan startTime) ||
         !TimeSpan.TryParseExact(txtBoxDrinkEnd.Text, @"hh\:mm", null, out TimeSpan endTime))
            {
                MessageBox.Show("Zadejte čas ve formátu HH:mm (např. 13:00)", "Chybný čas", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TimeSpan rozdil = endTime - startTime;
            if (rozdil.TotalMinutes <= 0)
            {
                MessageBox.Show("Konec pití musí být později než začátek!", "Chybný časový interval", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            int hodiny = (int)rozdil.TotalHours;
            int minuty = rozdil.Minutes;
            MessageBox.Show($"Rozdíl: {hodiny}h {minuty}min");



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

        


        private void Gender_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == CheckBoxMuz && CheckBoxMuz.IsChecked == true)
                CheckBoxZena.IsChecked = false;
            else if (sender == CheckBoxZena && CheckBoxZena.IsChecked == true)
                CheckBoxMuz.IsChecked = false;
        }   // pouze zobrazeni dalsich elementu po zadani inputu + kontorla spravnosti dat
        private string GetSelectedDrink()
        {
            return (DrinkComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? DrinkComboBox.SelectedItem?.ToString();
        }
        private void DrinkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrinkComboBox.SelectedItem == null)
            {
                AlcButton1.Visibility = Visibility.Collapsed;
                AlcButton2.Visibility = Visibility.Collapsed;
                AlcButton3.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                AlcButton1.Visibility = Visibility.Visible;
                AlcButton2.Visibility = Visibility.Visible;
                AlcButton3.Visibility = Visibility.Visible;
            }
            string selectedDrink = GetSelectedDrink();
            if (string.IsNullOrEmpty(selectedDrink)) return;
            List<double> volumes;
            if (!drinkVolumes.TryGetValue(selectedDrink, out volumes))
            {
                volumes = drinkVolumes["Default"];
            }
            AlcButton1.Content = $"{volumes[0]:0.##} l";
            AlcButton2.Content = $"{volumes[1]:0.##} l";
            AlcButton3.Content = $"{volumes[2]:0.##} l";

            currentButtonVolumes = volumes;
        }

        private void AlcButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void comboBoxWeight_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxWeight.Text == "kg")
            {
                isKilogram = true;
            }
            else isKilogram = false;
        }
    }
}