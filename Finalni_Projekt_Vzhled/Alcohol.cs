using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cteniDatTest
{
    [Serializable()]
    public class Alcohol
    {
        string name;
        double abv;
        string type;

        public Alcohol(string name, double abv, string type)
        {
            this.Name = name;
            this.Abv = abv;
            this.type = type;

        }
        public Alcohol() { }

        [XmlAttribute("Name")]
        public string Name { get => name; set => name = value; }

        [XmlAttribute("Abv")]
        public double Abv { get => abv; set => abv = value; }

        [XmlAttribute("Type")]
        public string Type { get => type; set => type = value; }

        public override string ToString() // Výstup do listboxu "listboxData"
        {
            return $"Name: {Name}\nAbv: {Abv}\nType: {Type}";
        }
    }
}
