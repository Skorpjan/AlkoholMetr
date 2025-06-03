using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

namespace cteniDat
{
    [Serializable]
    [XmlRoot("Database")]
    public class Database
    {
        [XmlArray("Alcohols")]
        [XmlArrayItem("Alcohol")]
        public BindingList<Alcohol> Alcohols { get; set; } = new();

        public BindingList<Alcohol> GetAll() => Alcohols ?? new();
    }

    [Serializable]
    public class Alcohol
    {
        string name;
        double abv;
        string type;

        public Alcohol(string name, double abv, string type)
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
            $"{Name}";
    }
}