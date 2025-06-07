using cteniDat;
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


namespace Finalni_Projekt_Vzhled
{
    public partial class SummaryWindow : Window
    {
        private Dictionary<string, double> totalByName = new();
        private double totalGrams = 0; // Celkové množství alkoholu v gramech, které bude předáno do výpočtu promile

        public double TotalAlcoholGrams { get; private set; } = 0; // Celkové množství alkoholu v gramech, které bude předáno do výpočtu promile

        public SummaryWindow()
        {
            InitializeComponent();
        }

        public void AddDrink(Alcohol alcohol, double volumeLiters) // Přidá nápoj do souhrnu - přepočítá objem alkoholu na gramy a aktualizuje celkové hodnoty
        {
            double grams = CalculateAlcoholGrams(alcohol, volumeLiters); // Přepočítá objem alkoholu na gramy

            if (!totalByName.ContainsKey(alcohol.Name)) // Pokud název alkoholu ještě není v slovníku, přidá ho
                totalByName[alcohol.Name] = 0;

            totalByName[alcohol.Name] += grams; // Přičte gramy k celkovému množství pro daný název alkoholu
            totalGrams += grams; //

            RefreshUI(); // Aktualizuje uživatelské rozhraní s celkovými hodnotami alkoholu
        }
        private void RefreshUI() // Aktualizuje uživatelské rozhraní s celkovými hodnotami alkoholu
        {
            listBoxPerType.Items.Clear(); // Vymaže předchozí položky v ListBoxu
            foreach (var pair in totalByName) // Pro každý pár (název alkoholu, množství v gramech) v slovníku
            {
                listBoxPerType.Items.Add($"{pair.Key}: {pair.Value:0.0} g"); // Přidá položku do ListBoxu s formátováním na 1 desetinné místo
            }

            labelTotalGrams.Content = $"Celkem: {totalGrams:0.0} g"; // Aktualizuje Label s celkovým množstvím alkoholu v gramech, formátováno na 1 desetinné místo
        }

        private double CalculateAlcoholGrams(Alcohol alcohol, double liters) // Přepočítá objem alkoholu na gramy - přepočet z objemu v litrech na gramy
        {
            double abv = alcohol.Abv;
            double density = 789;
            return liters * abv * density;
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)    // Pokračovat do výpočtu - přepočítá promile a zobrazí výsledek - po kliknutí na tlačítko "Pokračovat" v SummaryWindow
        {
            TotalAlcoholGrams = totalGrams;  // Uloží celkové množství alkoholu pro další výpočty

            if (Owner is MainWindow main)
            {

                try
                {
                    if (!TimeSpan.TryParse(main.txtBoxDrinkStart.Text, out TimeSpan startSpan)) // Kontrola formátu času začátku pití
                    {
                        MessageBox.Show("Neplatný formát času začátku pití. Použij např. 20:30", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!TimeSpan.TryParse(main.txtBoxDrinkEnd.Text, out TimeSpan endSpan)) // Kontrola formátu času konce pití
                    {
                        MessageBox.Show("Neplatný formát času konce pití. Použij např. 22:15", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!double.TryParse(main.txtBoxWeight.Text, out double weight))
                    {
                        MessageBox.Show("Neplatná hmotnost. Zadej číslo.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    DateTime startTime = DateTime.Today.Add(startSpan); // Převod TimeSpan na DateTime s použitím dnešního data
                    DateTime endTime = DateTime.Today.Add(endSpan);// Převod TimeSpan na DateTime s použitím dnešního data
                    if (endTime <= startTime)
                    {
                        MessageBox.Show("Čas konce pití musí být po začátku.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    string jednotka = ((ComboBoxItem)main.comboBoxUnit.SelectedItem).Content.ToString(); // Získání vybrané jednotky z ComboBoxu
                    if (jednotka == "lbs") //
                    {
                        weight *= 0.45359237; // převod na kg
                    }

                    var gender = main.CheckBoxMuz.IsChecked == true // Kontrola, zda je zaškrtnuto pohlaví muže
                        ? Calculation.Gender.Male
                        : Calculation.Gender.Female;

                    var calc = new Calculation(TotalAlcoholGrams, weight, startTime, endTime, gender); // Vytvoření instance Calculation s celkovým množstvím alkoholu, hmotností, časy a pohlavím

                    main.ZobrazitVysledek(calc); // Zobrazení výsledku v hlavním okně
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba při výpočtu: " + ex.Message);
                }
            }

            this.Close();
        }

    }

}

