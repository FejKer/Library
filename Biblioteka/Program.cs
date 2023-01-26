namespace Biblioteka
{
    internal class Program
    {
        static void Main(string[] args)
        {
            printMenu("");
        }

        public static void printMenu(string message)
        {
            Console.Clear();
            Console.WriteLine("====================Biblioteka====================");
            Console.WriteLine("1. Dodaj zasób");
            Console.WriteLine("2. Dodaj klienta");
            Console.WriteLine("3. Wypisz zasoby");
            Console.WriteLine("4. Wypisz klientów");
            Console.WriteLine("5. Wypożycz zasób");
            Console.WriteLine("6. Usuń zasoby biblioteki");
            Console.WriteLine("7. Zwróć wypożyczony zasób");
            Console.WriteLine("8. Zapisz stan bazy do pliku");
            Console.WriteLine("9. Odczytaj stan bazy z pliku");
            Console.WriteLine("==================================================");
            Console.WriteLine("\n" + message);

            int choice = 0;
            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                printMenu("Nieprawidłowy wybór");
            }

            switch (choice)
            {
                case 1: addResource(); break;
                case 2: addCustomer(); break;
                case 3: printResources(); break;
                case 4: printCustomers(); break;
                case 5: rentResource(); break;
                case 6: removeResource(); break;
                case 7: returnResource(); break;
                case 8: FileHandler.writeFile(); printMenu("Zapisano stan do pliku"); printMenu(""); break;
                case 9: FileHandler.readFile(); printMenu("Odczytano stan z pliku"); printMenu(""); break;
                default: printMenu("Nieprawidłowy wybór"); break;
            }
        }

        static void printCustomers()
        {
            string s = "";
            if (Database.GetCustomers().Count != 0)
            {
                foreach (var customer in Database.GetCustomers())
                {
                    s += customer;
                    s += "\n";
                }
            }
            else
            {
                printMenu("Brak klientów w bazie.");
            }
            printMenu(s);
        }

        static void printResources()
        {
            string s = "";
            if (Database.GetResources().Count != 0)
            {
                foreach (var resource in Database.GetResources())
                {
                    if (resource.isDeleted)
                    {
                        continue;
                    }
                    s += resource;
                    s += "\n";
                    s += "\n";
                }
                s += "\n";
                foreach (var quantity in Database.GetQuantities())
                {
                    s += "Kod kreskowy: " + quantity.Key + "\nIlość egzemplarzy: " + quantity.Value;
                    s += "\n";
                }

            }
            else
            {
                printMenu("Brak zasobów w bazie.");
            }
            printMenu(s);
        }

        static void addResource()
        {
            Console.WriteLine("1. Książka");
            Console.WriteLine("2. Komiks");
            Console.WriteLine("3. Czasopismo");
            int choice = 0;
            try
            {
                choice = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception ex)
            {
                printMenu("Nieprawidłowy wybór");
            }

            switch (choice)
            {
                case 1: addBook(); break;
                case 2: addComicBook(); break;
                case 3: addNewspaper(); break;
                default: printMenu("Nieprawidłowy wybór"); break;
            }

        }

        static void addBook()
        {
            try
            {
                Console.Write("Nazwa: ");
                string name = Console.ReadLine();
                Console.Write("Kod kreskowy: ");
                string barcode = Console.ReadLine();
                Console.Write("Autor: ");
                string description = Console.ReadLine();
                Console.Write("Rok wydania: ");
                string year = Console.ReadLine();
                Console.Write("Ilość stron: ");
                int pages = Convert.ToInt32(Console.ReadLine());
                Resource book = new Book(barcode, name, description, year, pages);
                Database.addResource(book);
                Database.updateQuantities(barcode, "+");
                printMenu("Utworzono zasób ID " + book.id);
            }
            catch (Exception ex)
            {
                printMenu("Błąd przy tworzeniu zasobu");
            }
        }

        static void addComicBook()
        {
            try
            {
                Console.Write("Nazwa: ");
                string name = Console.ReadLine();
                Console.Write("Kod kreskowy: ");
                string barcode = Console.ReadLine();
                Console.Write("Autor: ");
                string description = Console.ReadLine();
                Console.Write("Rok wydania: ");
                string year = Console.ReadLine();
                Console.Write("Ilość stron: ");
                string universe = Console.ReadLine();
                Resource comicBook = new ComicBook(barcode, name, description, year, universe);
                Database.addResource(comicBook);
                Database.updateQuantities(barcode, "+");
                printMenu("Utworzono zasób ID " + comicBook.id);
            }
            catch (Exception ex)
            {
                printMenu("Błąd przy tworzeniu zasobu");
            }
        }

        static void addNewspaper()
        {
            try
            {
                Console.Write("Nazwa: ");
                string name = Console.ReadLine();
                Console.Write("Kod kreskowy: ");
                string barcode = Console.ReadLine();
                Console.Write("Autor: ");
                string description = Console.ReadLine();
                Console.Write("Rok wydania: ");
                string year = Console.ReadLine();
                Console.Write("Wydawnictwo: ");
                string publicationCompany = Console.ReadLine();
                Resource newspaper = new Newspaper(barcode, name, description, year, publicationCompany);
                Database.addResource(newspaper);
                Database.updateQuantities(barcode, "+");
                printMenu("Utworzono zasób ID " + newspaper.id);
            }
            catch (Exception ex)
            {
                printMenu("Błąd przy tworzeniu zasobu");
            }
        }

        static void addCustomer()
        {
            Console.Write("Podaj login: ");
            string name = "";
            try
            {
                name = Console.ReadLine();
            }
            catch (Exception ex)
            {
                printMenu("Niepoprawny login");
            }

            if (name == null || name.Equals(""))
            {
                printMenu("Niepoprawny login");
            }

            foreach (var customer in Database.GetCustomers())
            {
                if (customer.name == name)
                {
                    printMenu("Login jest już w użyciu");
                }
            }

            Customer c = new Customer(name);
            Database.addCustomer(c);
            printMenu("Utworzono klienta ID: " + c.id + " " + c.name);
        }

        static void removeResource()
        {
            Console.Write("Podaj ID: ");
            try
            {
                int id = Convert.ToInt32(Console.ReadLine());

                foreach (var resource in Database.GetResources())
                {
                    if (resource.id == id)
                    {
                        if (!resource.isAvailable)
                        {
                            printMenu("Zasób jest aktualnie przez kogoś wypożyczony lub został już usunięty");
                        }
                        resource.isDeleted = true;
                        resource.isAvailable = false;
                        Database.updateQuantities(resource.barcode, "-");
                    }
                }

            }
            catch (Exception ex)
            {
                printMenu("Błąd przy usuwaniu zasobu");
            }
            printMenu("Usunięto zasób");
        }

        static void rentResource()
        {
            try
            {
                Console.Write("Podaj id klienta: ");
                int clientId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Podaj id zasobu: ");
                int resourceId = Convert.ToInt32(Console.ReadLine());
                Customer customer = null;
                Resource resource = null;
                foreach (var c in Database.GetCustomers())
                {
                    if (c.id == clientId)
                    {
                        customer = c;
                    }
                }
                foreach (var r in Database.GetResources())
                {
                    if (r.id == resourceId)
                    {
                        resource = r;
                    }
                }
                if (customer != null && resource != null)
                {
                    Resource.rentResource(customer, resource);
                    printMenu("Klient " + customer.name + " wypożyczył " + resource.name);
                }
                else
                {
                    printMenu("Błąd przy wypożyczaniu");
                }

            }
            catch (Exception ex)
            {
                printMenu("Błąd przy wypożyczaniu");
            }

        }

        static void returnResource()
        {
            try
            {
                Customer customer = null;
                Console.Write("Podaj id klienta: ");
                int clientId = Convert.ToInt32(Console.ReadLine());
                bool found = false;
                foreach (var c in Database.GetCustomers())
                {
                    if (c.id == clientId)
                    {
                        customer = c;
                        found = true;
                        Console.WriteLine("Twoje wypożyczone zasoby:\n");
                        foreach (var r in c.borrowedResources)
                        {
                            Console.WriteLine(r);
                        }
                        break;
                    }
                }
                if (!found)
                {
                    printMenu("Nie znaleziono klienta");
                } else
                {
                    Console.Write("\nPodaj id zasobu do zwrotu: ");
                    int resourceId = Convert.ToInt32(Console.ReadLine());
                    foreach (var r in customer.borrowedResources)
                    {
                        if (r.id == resourceId)
                        {
                            Resource.returnResource(customer, r);
                            printMenu("Zwrócono zasób " + r.name);
                            break;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                printMenu("Bład przy zwracaniu zasobu");
            }
        }
    }
}