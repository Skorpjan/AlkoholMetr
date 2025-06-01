/*
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace cteniDat
{
    [Serializable]
    [XmlRoot("Database")]
    public class Database
    {
        [XmlArray("Alcohols")]
        [XmlArrayItem("Alcohol")]
        public List<Alcohol> Alcohols { get; set; }

        public List<Alcohol> GetAll() => Alcohols ?? new List<Alcohol>();
    }

    [Serializable]
    public class Alcohol
    {
        string name;
        double abv;
        string type;

        public Alcohol(string name, double abv, string type)
        {
            this.Name = name;
            this.Abv = abv;
            this.Type = type;
        }

        public Alcohol() { }

        [XmlAttribute("Name")]
        public string Name { get => name; set => name = value; }

        [XmlAttribute("Abv")]
        public double Abv { get => abv; set => abv = value; }

        [XmlAttribute("Type")]
        public string Type { get => type; set => type = value; }

        public override string ToString()
        {
            return $"Name: {Name}\nAbv: {Abv}\nType: {Type}";
        }
    }
}
*/
// nevyuzity kod duvodu pouziti BindingList pro databazi
// merged do Database.cs pro zjednoduseni pristupu k datum