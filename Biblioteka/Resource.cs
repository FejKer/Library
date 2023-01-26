using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    abstract class Resource
    {
        static int nextId;                 //id referencyjne do stosowania auto inkrementacji
        public int id { get; set; }             //id konkretnego egzemplarza
        public string barcode { get; set; }        //id utworu/książki np. kod kreskowy
        public string name { get; set; }           //tytuł
        public string description { get; set; }    //opis np. autor
        public string dateOfIssue { get; set; }     //data wydania
        public bool isAvailable { get; set; }       //czy dostępne do wypożyczenia
        public bool isDeleted { get; set; }       //czy usunięte z bazy
        public Customer? customer { get; set; }     //klient, który wypożyczył zasób

        protected Resource(string barcode, string name, string description, string dateOfIssue)
        {
            this.id = Interlocked.Increment(ref nextId);
            this.barcode = barcode;
            this.name = name;
            this.description = description;
            this.dateOfIssue = dateOfIssue;
            this.isAvailable = true;
            this.isDeleted = false;
        }

        public static void rentResource(Customer customer, Resource resource)
        {
            if (!resource.isAvailable || resource.isDeleted)
            {
                Program.printMenu("Błąd przy wypożyczaniu");
            }

            resource.isAvailable = false;
            resource.customer = customer;
            customer.addResource(resource);
        }

        public static void returnResource(Customer customer, Resource resource)
        {
            if(customer != null)
            {
                customer.removeResource(resource);
                resource.customer = null;
                resource.isAvailable = true;
            }
        }

        public string getBarcode()
        {
            return barcode;
        }

    }
}
