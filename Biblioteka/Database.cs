using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    internal class Database
    {
        private static List<Resource> resources = new List<Resource>();                                            //zmienna przechowująca wszystkie zasoby
        private static List<Customer> customers = new List<Customer>();                                       //zmienna przechowująca wszystkich klientów
        private static Dictionary<string, int> quantities = new Dictionary<string, int>();                     //zmienna przechowująca ilości poszczególnych egzemplarzy w parze <kod kreskowy, ilosc>

        public static void updateQuantities(string barcode, string op)
        {
            if(!quantities.ContainsKey(barcode))
            {
                quantities.Add(barcode, 1);
                return;
            }

            if (op == "+")
            {
                quantities[barcode]++;
            } else if (op == "-" && quantities[barcode] != 0)
            {
                quantities[barcode]--;
            } else
            {
                Console.WriteLine("Nieprawidłowy operator");
            }
        }

        public static void addCustomer(Customer customer)
        {
            customers.Add(customer);
        }

        public static void addResource(Resource resource)
        {
            resources.Add(resource);
        }

        public static List<Resource> GetResources()
        {
            return resources;
        }

        public static List<Customer> GetCustomers()
        {
            return customers;
        }

        public static Dictionary<string, int> GetQuantities()
        {
            return quantities;
        }

        public static void SetResources(List<Resource> res)
        {
            resources = res;
        }

        public static void SetCustomers(List<Customer> cus)
        {
            customers = cus;
        }

        public static void SetQuantities(Dictionary<string, int> qua)
        {
            quantities = qua;
        }

    }
}
