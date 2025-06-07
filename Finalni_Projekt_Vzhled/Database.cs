using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
// nesahat!!!
namespace cteniDat
{
    [Serializable]
    [XmlRoot("Database")]
    public class Database
    {
        [XmlArray("Alcohols")]
        [XmlArrayItem("Alcohol")]
        public BindingList<Alcohol> Alcohols { get; set; } = new();// BindingList pro automatickou aktualizaci UI při změně dat

        public BindingList<Alcohol> GetAll() => Alcohols ?? new(); // Vrací všechny alkoholy z databáze, nebo prázdný seznam, pokud žádné nejsou
    }

    [Serializable]
    public class Alcohol
    {
        string name;
        double abv;
        string type;

        public Alcohol(string name, double abv, string type) // Konstruktor pro vytvoření instance alkoholu s názvem, obsahem alkoholu a typem
        {
            Name = name;
            Abv = abv;
            Type = type;
        }

        public Alcohol() { }

        [XmlAttribute("Name")]
        public string Name { get => name; set => name = value; }

        [XmlAttribute("Abv")]
        public double Abv { get => abv; set => abv = value; }

        [XmlAttribute("Type")]
        public string Type { get => type; set => type = value; }

        public override string ToString() =>
            $"Name: {Name}\nAbv: {Abv}\nType: {Type}";
    }
}