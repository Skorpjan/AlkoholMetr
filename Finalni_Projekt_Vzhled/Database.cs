using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace cteniDatTest
{
    [Serializable]
    [XmlRoot("Database")]
    public class Database
    {
        [XmlArray("Alcohols"), XmlArrayItem("Alcohol")]
        public BindingList<Alcohol> Alcohols { get; set; } = new BindingList<Alcohol>();

        public Database() { }

        public BindingList<object> GetAll() //vytvoří novou BindingList<object> a zkopíruje do ní všechny položky z Alcohols. Hodí se, když potřebujete předat jednotný seznam různě typovaných položek
        {
            var all = new BindingList<object>();
            foreach (var p in Alcohols) all.Add(p);
            return all;
        }
    }
}
