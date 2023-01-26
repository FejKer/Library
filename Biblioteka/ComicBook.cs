using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    class ComicBook : Resource
    {
        public string universe;
        public ComicBook(string barcode, string name, string description, string dateOfIssue, string universe) : base(barcode, name, description, dateOfIssue)
        {
            this.universe = universe;
        }

        public override string ToString()
        {
            if (customer == null)
            {
                return "Komiks: " + id + "\nTytuł: " + name + "\nKod kreskowy: " + barcode + "\nAutor: " + description + "\nRok wydania: " + dateOfIssue + "\nUniwersum: " + universe + "\nDostępna do wypożyczenia";
            }
            return "Komiks: " + id + "\nTytuł: " + name + "\nKod kreskowy: " + barcode + "\nAutor: " + description + "\nRok wydania: " + dateOfIssue + "\nUniwersum: " + universe + "\nWypożyczający: " + customer.GetName();
        }
    }
}
