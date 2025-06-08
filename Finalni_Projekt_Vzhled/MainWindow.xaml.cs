
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
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using static Finalni_Projekt_Vzhled.Calculation; // Corrected to use 'using static' for the Calculation type
namespace Finalni_Projekt_Vzhled
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>77
    public partial class MainWindow : Window
    {
        private  cteniDat.Database data; // instance databaze, ktera obsahuje vsechny alkoholy
        private List<Alcohol> allAlcohols = new(); // seznam vsech alkoholu z databaze
        private readonly Dictionary<string, List<double>> drinkVolumes = new()
        {
        { "Beer", new List<double> { 0.3, 0.5, 1.0 } },
        { "Distillate", new List<double> { 0.02, 0.04, 0.05 } },
        { "Vine", new List<double> { 0.1, 0.2, 0.3 } },
        { "Default", new List<double> { 0.1, 0.3, 0.5 } }
        }; //ke danym typum vzdy alkohlou pripojime vzdy typickou velikost servirovani
        private List<double> currentButtonVolumes = new() { 0.1, 0.3, 0.5 }; // default volumes for buttons if no specific type is selected

        Alcohol? selectedAlcohol = null;

        private SummaryWindow? summaryWindow; // instance summary okna, ktere se otevre po pridani alkoholu do summary

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

            if (!File.Exists(path)) // kontrola, zda soubor data.xml existuje
            {
                MessageBox.Show("data.xml not found!");
                data = new Database(); 
                return;
            }

            try
            {
                var serializer = new XmlSerializer(typeof(Database)); // vytvoreni serializeru pro deserializaci dat z data.xml
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read); // otevreni souboru data.xml pro cteni
                data = (Database)serializer.Deserialize(fs); // deserializace dat z data.xml do instance databaze
                allAlcohols = data.Alcohols.ToList(); // ziskani vsech alkoholu z databaze a jejich ulozeni do seznamu
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load data.xml: {ex.Message}");
                data = new Database(); 
            }
        }
        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = FilterTextBox.Text.Trim().ToLower(); // ziskani textu z textboxu pro filtraci alkoholu, odstraneni mezery na zacatku a konci a prevedeni na mala pismena

            var filtered = allAlcohols
                .Where(a => a.Name.ToLower().Contains(filter))
                .ToList(); // filtrujeme seznam alkoholu podle jmena, pokud jmeno obsahuje text z textboxu, prevedeme na mala pismena pro porovnani

            DrinkComboBox.ItemsSource = filtered;
        } //metoda pro filtraci alkoholu podle jmena



        private void Gender_Checked(object sender, RoutedEventArgs e) // metoda pro kontrolu, zda je vybrano pohlavi, pokud ano, druhe se odznačí
        {
            if (sender == CheckBoxMuz && CheckBoxMuz.IsChecked == true)
                CheckBoxZena.IsChecked = false;
            else if (sender == CheckBoxZena && CheckBoxZena.IsChecked == true)
                CheckBoxMuz.IsChecked = false;
        }   // pouze zobrazeni dalsich elementu po zadani inputu + kontorla spravnosti dat

        private void DrinkComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DrinkComboBox.SelectedItem == null) // pokud neni vybrany zadny alkohol, skryjeme tlacitka s objemy
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
            string selectedDrinkType = (DrinkComboBox.SelectedItem as Alcohol)?.Type ?? "Default"; // ziskani typu vybraneho alkoholu, pokud neni vybrany, pouzijeme "Default"
            selectedAlcohol = DrinkComboBox.SelectedItem as Alcohol; // ulozeni vybraneho alkoholu pro dalsi pouziti
            if (!drinkVolumes.TryGetValue(selectedDrinkType, out List<double> volumes)) // ziskani objemu pro vybrany typ alkoholu, pokud neni nalezen, pouzijeme "Default"
            {
                volumes = drinkVolumes["Default"];
            }

            AlcButton1.Content = $"{volumes[0]:0.##} l"; // nastavime obsah tlacitek s objemy podle typu alkoholu
            AlcButton2.Content = $"{volumes[1]:0.##} l";
            AlcButton3.Content = $"{volumes[2]:0.##} l";

            currentButtonVolumes = volumes; // ulozime aktualni objemy pro dalsi pouziti
        }

        private void AlcButtonAdd_Click(object sender, RoutedEventArgs e)   // metoda pro pridani alkoholu do summary okna
        {
            if (DrinkComboBox.SelectedItem is not Alcohol selectedAlcohol) // kontrola, zda je vybrany alkohol
                return;

            int index = -1; //default index pro kontrolu, zda je vybrano tlacitko s objemem
            if (AlcButton1.IsChecked == true) index = 0; // kontrola, zda je vybrano tlacitko s objemem 1
            else if (AlcButton2.IsChecked == true) index = 1; // kontrola, zda je vybrano tlacitko s objemem 2
            else if (AlcButton3.IsChecked == true) index = 2; // kontrola, zda je vybrano tlacitko s objemem 3

            if (index == -1)// pokud neni vybrano zadne tlacitko s objemem, zobrazime hlasku
            {
                MessageBox.Show("Vyber objem nápoje!");
                return;
            }

            double volume = currentButtonVolumes[index];// ziskani objemu z tlacitka
            if (summaryWindow == null || !summaryWindow.IsLoaded)// pokud neni otevreno summary okno, vytvorime nove
            {
                summaryWindow = new SummaryWindow();
                summaryWindow.Owner = this;// nastavime vlastnika summary okna na hlavni okno
                summaryWindow.Show();// zobrazime summary okno
            }
            DrinkComboBox.SelectedItem = selectedAlcohol; // aby se porad numusel yybirat alkohol znovu
            // znovu nastavime vybrany alkohol, aby se aktualizoval v summary okne
            summaryWindow.AddDrink(selectedAlcohol, volume);
            AlcButton1.IsChecked = false;// odznačíme všechna tlačítka s objemem
            AlcButton2.IsChecked = false;
            AlcButton3.IsChecked = false;
        }
        private void AlcButton_Checked(object sender, RoutedEventArgs e)//
        {
            foreach (var btn in new[] { AlcButton1, AlcButton2, AlcButton3 }) //projde vsechny tlactika s objemem, pokud neni tohle tlacitko, ktere bylo kliknuto, odznačí ho
            {
                if (btn != sender)
                    btn.IsChecked = false;
            }
        }

        public void ZobrazitVysledek(Calculation calc) // metoda pro zobrazeni vysledku v hlavnim okne
        {
            double promileStart = calc.PromileAtStart; //graf
            double timeToZeroHours = (calc.SoberTimeEstimate - calc.EndTime).TotalHours;

            var graf = new Graf(promileStart, timeToZeroHours);
            this.DataContext = graf;
            GrafPlotView.Model = graf.GrafModel;
            GrafPlotView.InvalidatePlot(true);

            TextPromileResult.Text = $"{calc.PromileAtEnd:0.00}‰ na konci pití"; // zobrazi promile na konci pití

            int hodiny = (int)calc.EliminationDuration.TotalHours; // ziskani hodin z doby odbourani
            int minuty = calc.EliminationDuration.Minutes; // ziskani minut z doby odbourani

            TextSoberTime.Text = $"Za {hodiny}h {minuty}m budete mít 0‰ (cca {calc.SoberTimeEstimate:HH:mm})"; // zobrazi dobu odbourani a odhad casu, kdy bude uzivatel střízlivý

            // Zviditelni výsledek
            TextPromileResult.Visibility = Visibility.Visible; // zviditelni text s promile
            TextSoberTime.Visibility = Visibility.Visible; // zviditelni text s dobou odbourani
        }

    }
}