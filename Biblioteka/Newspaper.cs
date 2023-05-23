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

        public Newspaper(string barcode, string name, string description, string dateOfIssue, string publicationCompany) : base(barcode, name, description, dateOfIssue)
        {
            this.publicationCompany = publicationCompany;
        }

        public override string ToString()
        {
            if (Customer == null)
            {
                return "Czasopismo: " + Id + "\nTytuł: " + Name + "\nKod kreskowy: " + Barcode + "\nAutor: " + Description + "\nRok wydania: " + DateOfIssue + "\nWydawnictwo: " + publicationCompany + "\nDostępna do wypożyczenia";
            }
            return "Czasopismo: " + Id + "\nTytuł: " + Name + "\nKod kreskowy: " + Barcode + "\nAutor: " + Description + "\nRok wydania: " + DateOfIssue + "\nWydawnictwo: " + publicationCompany + "\nWypożyczający: " + Customer.GetName();
        }
    }
}
