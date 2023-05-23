using System.Collections.Generic;

namespace Biblioteka
{
    internal static class Database
    {
        private static List<Resource> resources = new List<Resource>();
        private static List<Customer> customers = new List<Customer>();
        private static Dictionary<string, int> quantities = new Dictionary<string, int>();

        public static void UpdateQuantities(string barcode, string op)
        {
            if (!quantities.ContainsKey(barcode))
            {
                quantities.Add(barcode, 1);
                return;
            }

            if (op == "+")
            {
                quantities[barcode]++;
            }
            else if (op == "-" && quantities[barcode] != 0)
            {
                quantities[barcode]--;
            }
            else
            {
                MenuHelper.PrintMenu("Nieprawidłowy operator");
            }
        }

        public static void AddCustomer(Customer customer)
        {
            customers.Add(customer);
        }

        public static void AddResource(Resource resource)
        {
            resources.Add(resource);
        }

        public static List<Resource> GetResources()
        {
            return resources;
        }

        public static void SetResources(List<Resource> r)
        {
            resources = r;
        }

        public static List<Customer> GetCustomers()
        {
            return customers;
        }

        public static void SetCustomers(List<Customer> c)
        {
            customers = c;
        }


        public static Dictionary<string, int> GetQuantities()
        {
            return quantities;
        }

        public static void SetQuantities(Dictionary<string, int> q)
        {
            quantities = q;
        }
    }
}
