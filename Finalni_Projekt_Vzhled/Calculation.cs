using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finalni_Projekt_Vzhled
{
    public class Calculation
    {
        public enum Gender // Definice výčtového typu pro pohlaví uživatele
        {
            Male,
            Female
        }

        public double AlcoholGrams { get; }
        public double WeightKg { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public Gender UserGender { get; }

        public double PromileAtEnd { get; private set; } 
        public TimeSpan EliminationDuration { get; private set; }
        public DateTime SoberTimeEstimate { get; private set; }

        public Calculation(double alcoholGrams, double weightKg, DateTime startTime, DateTime endTime, Gender gender)  
        {
            AlcoholGrams = alcoholGrams; // Přiřazení množství alkoholu v gramech
            WeightKg = weightKg; // Přiřazení hmotnosti uživatele v kg
            StartTime = startTime; // Přiřazení času začátku pití
            EndTime = endTime; // Přiřazení času začátku a konce pití
            UserGender = gender;  // Přiřazení pohlaví uživatele

            Calculate(); // Spuštění výpočtu při vytvoření instance
        }

        private void Calculate()
        {
            double r = UserGender == Gender.Male ? 0.68 : 0.55; // Redukční faktor pro muže a ženy
            double durationHours = (EndTime - StartTime).TotalHours;        // Doba pití v hodinách
            if (durationHours < 0) durationHours = 0; // Zabránění záporné době pití

            double bac = AlcoholGrams / (WeightKg * r);       // Výpočet promile
            double eliminationRate = 0.15;                    // Promile odbourané za hodinu
            double eliminated = eliminationRate * durationHours; // Kolik promile bylo odbouráno za dobu pití

            PromileAtEnd = bac - eliminated;        // Promile na konci doby pití
            if (PromileAtEnd < 0) PromileAtEnd = 0; // Zabránění záporným hodnotám promile

            double timeToSober = PromileAtEnd / eliminationRate; // Doba potřebná k odbourání promile
            EliminationDuration = TimeSpan.FromHours(timeToSober); // Doba odbourání v čase
            SoberTimeEstimate = EndTime.Add(EliminationDuration); // Odhad času
        }
    }
}