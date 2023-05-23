using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteka
{
    internal static class CustomerHelper
    {
        public static void PrintCustomers()
        {
            var customers = Database.GetCustomers();

            if (customers.Count == 0)
            {
                MenuHelper.PrintMenu("Brak klientów w bazie.");
                return;
            }

            string output = "";

            foreach (var customer in customers)
            {
                output += customer + "\n";
            }

            MenuHelper.PrintMenu(output);
        }

        public static void AddCustomer()
        {
            Console.Write("Podaj login: ");
            string name = Console.ReadLine();

            if (string.IsNullOrEmpty(name))
            {
                MenuHelper.PrintMenu("Niepoprawny login");
                return;
            }

            var customers = Database.GetCustomers();

            foreach (var customer in customers)
            {
                if (customer.Name == name)
                {
                    MenuHelper.PrintMenu("Login jest już w użyciu");
                    return;
                }
            }

            var newCustomer = new Customer(name);
            Database.AddCustomer(newCustomer);

            MenuHelper.PrintMenu($"Utworzono klienta ID: {newCustomer.Id} {newCustomer.Name}");
        }
    }
}
