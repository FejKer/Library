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
            if (customer == null)
            {
                return "Czasopismo: " + id + "\nTytuł: " + name + "\nKod kreskowy: " + barcode + "\nAutor: " + description + "\nRok wydania: " + dateOfIssue + "\nWydawnictwo: " + publicationCompany + "\nDostępna do wypożyczenia";
            }
            return "Czasopismo: " + id + "\nTytuł: " + name + "\nKod kreskowy: " + barcode + "\nAutor: " + description + "\nRok wydania: " + dateOfIssue + "\nWydawnictwo: " + publicationCompany + "\nWypożyczający: " + customer.GetName();
        }
    }
}
