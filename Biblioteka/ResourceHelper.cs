using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblioteka
{
    internal static class ResourceHelper
    {
        public static void PrintResources()
        {
            var resources = Database.GetResources();

            if (resources.Count == 0)
            {
                MenuHelper.PrintMenu("Brak zasobów w bazie.");
                return;
            }

            string output = "";

            foreach (var resource in resources)
            {
                if (resource.IsDeleted)
                {
                    continue;
                }

                output += resource + "\n\n";
            }

            var quantities = Database.GetQuantities();

            foreach (var quantity in quantities)
            {
                output += "Kod kreskowy: " + quantity.Key + "\nIlość egzemplarzy: " + quantity.Value + "\n\n";
            }

            MenuHelper.PrintMenu(output);
        }

        public static void AddResource()
        {
            Console.WriteLine("1. Książka");
            Console.WriteLine("2. Komiks");
            Console.WriteLine("3. Czasopismo");

            int choice;
            bool isValidChoice = int.TryParse(Console.ReadLine(), out choice);

            if (!isValidChoice)
            {
                MenuHelper.PrintMenu("Nieprawidłowy wybór");
                return;
            }

            switch (choice)
            {
                case 1:
                    AddBook();
                    break;
                case 2:
                    AddComicBook();
                    break;
                case 3:
                    AddNewspaper();
                    break;
                default:
                    MenuHelper.PrintMenu("Nieprawidłowy wybór");
                    break;
            }
        }

        private static void AddBook()
        {
            Console.Write("Nazwa: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                MenuHelper.PrintMenu("Nieprawidłowa nazwa");
                return;
            }

            Console.Write("Kod kreskowy: ");
            string barcode = Console.ReadLine();
            if (string.IsNullOrEmpty(barcode))
            {
                MenuHelper.PrintMenu("Nieprawidłowy kod kreskowy");
                return;
            }

            Console.Write("Autor: ");
            string description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
            {
                MenuHelper.PrintMenu("Nieprawidłowy autor");
                return;
            }

            Console.Write("Rok wydania: ");
            string year = Console.ReadLine();
            if (string.IsNullOrEmpty(year))
            {
                MenuHelper.PrintMenu("Nieprawidłowy rok wydania");
                return;
            }

            Console.Write("Ilość stron: ");
            if (!int.TryParse(Console.ReadLine(), out int pages) || pages <= 0)
            {
                MenuHelper.PrintMenu("Nieprawidłowa liczba stron");
                return;
            }

            var book = new Book(barcode, name, description, year, pages);
            Database.AddResource(book);
            Database.UpdateQuantities(barcode, "+");
            MenuHelper.PrintMenu("Utworzono zasób ID " + book.Id);
        }

        private static void AddComicBook()
        {
            Console.Write("Nazwa: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                MenuHelper.PrintMenu("Nieprawidłowa nazwa");
                return;
            }

            Console.Write("Kod kreskowy: ");
            string barcode = Console.ReadLine();
            if (string.IsNullOrEmpty(barcode))
            {
                MenuHelper.PrintMenu("Nieprawidłowy kod kreskowy");
                return;
            }

            Console.Write("Autor: ");
            string description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
            {
                MenuHelper.PrintMenu("Nieprawidłowy autor");
                return;
            }

            Console.Write("Rok wydania: ");
            string year = Console.ReadLine();
            if (string.IsNullOrEmpty(year))
            {
                MenuHelper.PrintMenu("Nieprawidłowy rok wydania");
                return;
            }

            Console.Write("Ilość stron: ");
            string universe = Console.ReadLine();
            if (string.IsNullOrEmpty(universe))
            {
                MenuHelper.PrintMenu("Nieprawidłowe uniwersum");
                return;
            }

            var comicBook = new ComicBook(barcode, name, description, year, universe);
            Database.AddResource(comicBook);
            Database.UpdateQuantities(barcode, "+");
            MenuHelper.PrintMenu("Utworzono zasób ID " + comicBook.Id);
        }

        private static void AddNewspaper()
        {
            Console.Write("Nazwa: ");
            string name = Console.ReadLine();
            if (string.IsNullOrEmpty(name))
            {
                MenuHelper.PrintMenu("Nieprawidłowa nazwa");
                return;
            }

            Console.Write("Kod kreskowy: ");
            string barcode = Console.ReadLine();
            if (string.IsNullOrEmpty(barcode))
            {
                MenuHelper.PrintMenu("Nieprawidłowy kod kreskowy");
                return;
            }

            Console.Write("Autor: ");
            string description = Console.ReadLine();
            if (string.IsNullOrEmpty(description))
            {
                MenuHelper.PrintMenu("Nieprawidłowy autor");
                return;
            }

            Console.Write("Rok wydania: ");
            string year = Console.ReadLine();
            if (string.IsNullOrEmpty(year))
            {
                MenuHelper.PrintMenu("Nieprawidłowy rok wydania");
                return;
            }

            Console.Write("Wydawnictwo: ");
            string publicationCompany = Console.ReadLine();
            if (string.IsNullOrEmpty(publicationCompany))
            {
                MenuHelper.PrintMenu("Nieprawidłowe wydawnictwo");
                return;
            }

            var newspaper = new Newspaper(barcode, name, description, year, publicationCompany);
            Database.AddResource(newspaper);
            Database.UpdateQuantities(barcode, "+");
            MenuHelper.PrintMenu("Utworzono zasób ID " + newspaper.Id);
        }

        public static void RemoveResource()
        {
            Console.Write("Podaj ID: ");

            try
            {
                int id = int.Parse(Console.ReadLine());
                var resources = Database.GetResources();

                foreach (var resource in resources)
                {
                    if (resource.Id == id)
                    {
                        if (!resource.IsAvailable)
                        {
                            MenuHelper.PrintMenu("Zasób jest aktualnie przez kogoś wypożyczony lub został już usunięty");
                        }

                        resource.IsDeleted = true;
                        resource.IsAvailable = false;

                        Database.UpdateQuantities(resource.Barcode, "-");
                    }
                }
            }
            catch (Exception)
            {
                MenuHelper.PrintMenu("Błąd przy usuwaniu zasobu");
            }

            MenuHelper.PrintMenu("Usunięto zasób");
        }

        public static void RentResource()
        {
            Console.Write("Podaj ID klienta: ");
            int clientId = int.Parse(Console.ReadLine());
            Console.Write("Podaj ID zasobu: ");
            int resourceId = int.Parse(Console.ReadLine());
            Customer customer = null;
            Resource resource = null;
            foreach (Customer c in Database.GetCustomers()) {
                if (c.Id == clientId) customer = c;
            }
            foreach (Resource r in Database.GetResources())
            {
                if (r.Id == resourceId) resource = r;
            }
            if (customer == null || resource == null)
            {
                MenuHelper.PrintMenu("Błąd przy wypożyczaniu");
                return;
            }
            if (!resource.IsAvailable || resource.IsDeleted)
            {
                MenuHelper.PrintMenu("Błąd przy wypożyczaniu");
                return;
            }
            resource.IsAvailable = false;
            resource.Customer = customer;
            customer.AddResource(resource);
            MenuHelper.PrintMenu("Klient " + customer.Name + " wypożyczył: " + resource.Name);
        }

        public static void ReturnResource()
        {
            try
            {
                Console.Write("Podaj ID klienta: ");
                int clientId = int.Parse(Console.ReadLine());

                var customer = Database.GetCustomers().FirstOrDefault(c => c.Id == clientId);

                if (customer == null)
                {
                    MenuHelper.PrintMenu("Nie znaleziono klienta");
                    return;
                }

                Console.WriteLine("Twoje wypożyczone zasoby:\n");

                foreach (var resource in customer.BorrowedResources)
                {
                    Console.WriteLine(resource);
                }

                Console.Write("\nPodaj ID zasobu do zwrotu: ");
                int resourceId = int.Parse(Console.ReadLine());

                foreach (var resource in customer.BorrowedResources)
                {
                    if (resource.Id == resourceId)
                    {
                        Resource.ReturnResource(customer, resource);
                        MenuHelper.PrintMenu("Zwrócono zasób " + resource.Name);
                        break;
                    }
                }
            }
            catch (Exception)
            {
                MenuHelper.PrintMenu("Błąd przy zwracaniu zasobu");
            }
        }
    }
}
