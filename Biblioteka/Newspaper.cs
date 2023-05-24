using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    class Newspaper : Resource
    {
        public string publicationCompany;

        public Newspaper(string barcode, string name, string description, Location location, string dateOfIssue, string publicationCompany) : base(barcode, name, description, dateOfIssue,location)
        {
            this.publicationCompany = publicationCompany;
        }

        public override string ToString()
        {
            if (IsDamaged)
            {
                return "Czasopismo: " + Id + "\nTytuł: " + Name + "\nKod kreskowy: " + Barcode + "\nAutor: " + Description + "\nLokalizacja w bibliotece: " + Location + "\nRok wydania: " + DateOfIssue + "\nWydawnictwo: " + publicationCompany + "\nUszkodzony - niedostępny do wypożyczenia";
            }
            if (Customer == null)
            {
                return "Czasopismo: " + Id + "\nTytuł: " + Name + "\nKod kreskowy: " + Barcode + "\nAutor: " + Description + "\nLokalizacja w bibliotece: " + Location + "\nRok wydania: " + DateOfIssue + "\nWydawnictwo: " + publicationCompany + "\nDostępna do wypożyczenia";
            }
            return "Czasopismo: " + Id + "\nTytuł: " + Name + "\nKod kreskowy: " + Barcode + "\nAutor: " + Description + "\nRok wydania: " + DateOfIssue + "\nWydawnictwo: " + publicationCompany + "\nWypożyczający: " + Customer.GetName();
        }
    }
}
