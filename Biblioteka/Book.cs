using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    class Book : Resource
    {
        public int pages;          //dlugosc ksiazki w stronach

        public Book(string barcode, string name, string description, string dateOfIssue, int pages) : base(barcode, name, description, dateOfIssue)
        {
            this.pages = pages;
        }

        public override string ToString()
        {
            if (customer == null)
            {
                return "Książka: " + id + "\nTytuł: " + name + "\nKod kreskowy: " + barcode + "\nAutor: " + description + "\nRok wydania: " + dateOfIssue + "\nIlość stron: " + pages +"\nDostępna do wypożyczenia";
            }
            return "Książka: " + id + "\nTytuł: " + name + "\nKod kreskowy: " + barcode + "\nAutor: " + description + "\nRok wydania: " + dateOfIssue + "\nIlość stron: " + pages + "\nWypożyczający: " + customer.GetName();
        }
    }
}
