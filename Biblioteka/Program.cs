using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Biblioteka
{
    internal class Program
    {
        public class Zasoby
        {
            private static int last_resource_id;
            public int id { get; private set; }         //id konkretnego egzemplarza
            public string group_id { get; set; }           //id egzemplarza - kod kreskowy
            public string name { get; set; }            //nazwa egzemplarza
            public bool isBorrowed { get; set; }       //czy przedmiot mozna wypozyczyc?

            public Zasoby()
            {
                id = Interlocked.Increment(ref last_resource_id); //automatyczna inkrementacja id przy tworzeniu nowego obiektu
            }
        }

        public class Book : Zasoby
        {
            private readonly int pages;
            private readonly string author;
            public Book(string name, string group_id, int pages, string author)
            {
                this.name = name;
                this.group_id = group_id;
                this.pages = pages;
                this.author = author;
            }
        }

        public class Newspaper : Zasoby
        {
            private readonly string date;
            public Newspaper(string name, string group_id, string date)
            {
                this.name = name;
                this.group_id = group_id;
                this.date = date;
            }
        }

        public class Movie : Zasoby
        {
            private readonly int length;
            public Movie(string name, string group_id, int length)
            {
                this.name = name;
                this.group_id = group_id;
                this.length = length;
            }
        }

        public class ScientificWork : Zasoby
        {
            private readonly string author;
            public ScientificWork(string name, string group_id, string author)
            {
                this.name = name;
                this.group_id = group_id;
                this.author = author;
            }
        }

        public class Customer
        {
            private static int last_customer_id;
            public int id { get; private set; }
            public string name { get; private set; }
            public List<Zasoby> wypozyczone;
            public Customer(string name)
            {
                this.name = name;
                id = Interlocked.Increment(ref last_customer_id);                      //id klienta z automatyczną inkrementacją
                this.wypozyczone = new List<Zasoby>();                                 //wypozyczone zasoby
            }
        }

        static void Main(string[] args)
        {
            var program = new Program();
            var zasoby = new List<Zasoby>();                                            //zmienna przechowująca wszystkie zasoby
            var customers = new List<Customer>();                                       //zmienna przechowująca wszystkich klientów
            Dictionary<string, int> quantities = new Dictionary<string, int>();         //zmienna przechowująca ilości poszczególnych egzemplarzy
            
            program.printMainMenu(zasoby, quantities, customers);
        }

        void printMainMenu(dynamic zasoby, Dictionary<string, int> quantities, dynamic customers)
        {
            Console.Clear();
            Console.WriteLine("===================Biblioteka===================");
            Console.WriteLine("1.Wypożycz przedmiot.");
            Console.WriteLine("2.Dodaj nowy przedmiot.");
            Console.WriteLine("3.Usuń przedmiot.");
            Console.WriteLine("4.Zwróć przedmiot.");
            Console.WriteLine("5.Wyświetl zasoby.");
            Console.WriteLine("6.Zapisz stan do pliku.");
            Console.WriteLine("7.Wczytaj stan z pliku.");
            Console.WriteLine();
            int n = Convert.ToInt32(Console.ReadLine());                                   //czytamy wybor uzytkownika

            switch (n)
            {
                case 1: rentResource(customers, zasoby); printMainMenu(zasoby, quantities, customers); break;
                case 2: addResource(zasoby, quantities); printMainMenu(zasoby, quantities, customers); break;
                case 3: removeResource(zasoby, quantities); printMainMenu(zasoby, quantities, customers); break;
                case 4: rentResource(customers, zasoby); printMainMenu(zasoby, quantities, customers); break;
                case 5: printResources(zasoby, quantities); printMainMenu(zasoby, quantities, customers); break;
                default: Console.WriteLine("Niewłaściwy wybór."); printMainMenu(zasoby, quantities, customers); break;
            }
        }
        void rentResource(dynamic customers, dynamic zasoby)
        {
            Console.WriteLine("===================WYPOŻYCZENIE/ZWROT ZASOBU===================");
            Console.WriteLine("1. Nowy klient.");
            Console.WriteLine("2. Powracający klient.");
            int n = Convert.ToInt32(Console.ReadLine());
            switch (n)
            {
                case 1: AddCustomer(customers, zasoby); break;
                case 2: login(customers, zasoby); break;
                default: Console.WriteLine("Nieprawidłowy wybór."); rentResource(customers, zasoby); break;
            }
        }
        void AddCustomer(dynamic customers, dynamic zasoby)
        {
            Console.WriteLine("Wpisz login, którego chcesz używać:");
            string name = Console.ReadLine();

            foreach(Customer customer in customers)
            {
                if(customer.name == name)
                {
                    Console.WriteLine("Login jest już zajęty.");
                    Console.WriteLine("Naciśnij dowolny przycisk.");
                    Console.ReadKey();
                    AddCustomer(customers, zasoby);
                }
            }

            customers.Add(new Customer(name));
            Console.WriteLine("\nRejestracja pomyślna.");
            updateResource(zasoby, customers, name);
        }
        void login(dynamic customers, dynamic zasoby)
        {
            bool logged = false;
            Console.WriteLine("Podaj login.");
            string name = Console.ReadLine();
            foreach (Customer customer in customers)
            {
                if(customer.name == name)
                {
                    Console.WriteLine("Zalogowano");
                    logged = true;
                }
            }
            if (logged)
            {
                Console.WriteLine("1. Wypożyczenie.");
                Console.WriteLine("2. Zwrot.");
                int n = Convert.ToInt32(Console.ReadLine());
                if(n == 1)
                {
                    updateResource(zasoby, customers, name);
                } else if(n == 2)
                {
                    returnResource(zasoby, customers, name);
                }
                else
                {
                    Console.WriteLine("Niepoprawny wybór.");
                    login(customers, zasoby);
                }
            } 
            else
            {
                Console.WriteLine("Niepoprawny login.");
                Console.WriteLine("Naciśnij dowolny przycisk.");
                Console.ReadKey();
                login(customers, zasoby);
            }
        }
        void updateResource(dynamic zasoby, dynamic customers, string name)
        {
            Customer customer = null;
            Console.WriteLine("Podaj ID zasobu, który chcesz wypożyczyć.");
            int n = Convert.ToInt32(Console.ReadLine());
            foreach(Customer c in customers)
            {
                if(c.name == name)
                {
                    customer = c;
                }
            }
            foreach(Zasoby z in zasoby)
            {
                if (z.id == n && z.isBorrowed == false)
                {
                    z.isBorrowed = true;
                    customer.wypozyczone.Add(z);
                    Console.WriteLine("Wypożyczono zasób.");
                    Console.WriteLine("Naciśnij dowolny przycisk.");
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("Niepoprawny numer ID lub zasób jest już wypożyczony.");
                    updateResource(zasoby, customers, name);
                }
            }
        }
        void returnResource(dynamic zasoby, dynamic customers, string name)
        {
            Customer customer = null;
            foreach (Customer c in customers)
            {
                if (c.name == name)
                {
                    customer = c;
                }
            }
            foreach (Zasoby z in customer.wypozyczone)
            {
                Console.WriteLine("ID: " + z.id);
                Console.WriteLine("Nazwa: " + z.name);
            }
            Console.WriteLine("Podaj ID zasobu, który chcesz zwrócić.");
            int n = Convert.ToInt32(Console.ReadLine());
            foreach(Zasoby z in zasoby)
            {
                if(z.id == n)
                {
                    z.isBorrowed = false;
                    customer.wypozyczone.Remove(z);
                }
            }
        }
        void removeResource(dynamic zasoby, Dictionary<string, int> quantities)
        {
            Console.Clear();
            bool removed = false;
            string group_id = "";
            int id_to_remove;
            Console.WriteLine("===================USUWANIE ZASOBU===================");
            Console.WriteLine("Podaj id zasobu do usunięcia");
            id_to_remove = Convert.ToInt32(Console.ReadLine());
            int i = 0;
            foreach (Zasoby z in zasoby.ToArray())
            {
                if (z.id == id_to_remove)
                {
                    group_id = z.group_id;
                    zasoby.RemoveAt(i);
                    removed = true;
                }
                i++;
            }
            if (removed)
            {
                quantityHandlerRemove(quantities, group_id);
            }
            else
            {
                Console.WriteLine("Błąd podczas usuwania.\n Naciśnij dowolny klawisz.");
                Console.ReadKey();
            }
        }
        void printResources(dynamic zasoby, Dictionary<string, int> quantities)
        {
            Console.Clear();
            foreach (Zasoby z in zasoby)
            {
                int q;
                quantities.TryGetValue(z.group_id, out q);
                Console.WriteLine("===================================");
                Console.WriteLine("ID: " + z.id + "\nKod Kreskowy: " + z.group_id + "\nNazwa: " + z.name + "\nCzy wypożyczone? " + z.isBorrowed + "\nIlość egzemplarzy: " + q);
                Console.WriteLine("===================================");
            }
            Console.WriteLine();
            Console.WriteLine("Naciśnij dowolny klawisz.");
            Console.ReadKey();
        }
        void addResource(dynamic zasoby, Dictionary<string, int> quantities)
        {
            Console.Clear();
            Console.WriteLine("===================DODAWANIE ZASOBU===================");
            int choice;
            Console.WriteLine("1.Książka");
            Console.WriteLine("2.Czasopismo");
            Console.WriteLine("3.Film");
            Console.WriteLine("4.Praca naukowa");
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1: addBook(zasoby, quantities); break;
                case 2: addNewspaper(zasoby, quantities); break;
                case 3: addMovie(zasoby, quantities); break;
                case 4: addScientificWork(zasoby, quantities); break;
                default: Console.WriteLine("Niewłaściwy wybór."); addResource(zasoby, quantities); break;
            }
        }
        void addBook(dynamic zasoby, Dictionary<string, int> quantities)
        {
            int p;
            string s, a, n;
            Console.WriteLine("Podaj nazwę:");
            s = Console.ReadLine();
            Console.WriteLine("Podaj kod kreskowy:");
            n = Console.ReadLine();
            Console.WriteLine("Podaj ilosc stron:");
            p = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Podaj autora:");
            a = Console.ReadLine();
            zasoby.Add(new Book(s, n, p, a));
            quantityHandler(quantities, n);
        }
        void addNewspaper(dynamic zasoby, Dictionary<string, int> quantities)
        {
            string s, d, n;
            Console.WriteLine("Podaj nazwę:");
            s = Console.ReadLine();
            Console.WriteLine("Podaj kod kreskowy:");
            n = Console.ReadLine();
            Console.WriteLine("Podaj date wydania w formacie DD.MM.YYYY:");
            d = Console.ReadLine();
            zasoby.Add(new Newspaper(s, n, d));
            quantityHandler(quantities, n);
        }
        void addMovie(dynamic zasoby, Dictionary<string, int> quantities)
        {
            int l;
            string s, n;
            Console.WriteLine("Podaj nazwę:");
            s = Console.ReadLine();
            Console.WriteLine("Podaj kod kreskowy:");
            n = Console.ReadLine();
            Console.WriteLine("Podaj długość filmu w minutach:");
            l = Convert.ToInt32(Console.ReadLine());
            zasoby.Add(new Movie(s, n, l));
            quantityHandler(quantities, n);
        }
        void addScientificWork(dynamic zasoby, Dictionary<string, int> quantities)
        {
            string s, a, n;
            Console.WriteLine("Podaj nazwę:");
            s = Console.ReadLine();
            Console.WriteLine("Podaj kod kreskowy:");
            n = Console.ReadLine();
            Console.WriteLine("Podaj autora:");
            a = Console.ReadLine();
            zasoby.Add(new ScientificWork(s, n, a));
            quantityHandler(quantities, n);
        }
        void quantityHandler(Dictionary<string, int> quantities, string group_id)
        {
            try
            {
                quantities.Add(group_id, 1);                    // jeśli w zbiorze nie ma danego kodu kreskowego, dodajemy go i ustawiamy ilość egzemplarzy na 1
            }
            catch (Exception ex)
            {
                int temp = quantities[group_id] + 1;            // jeśli w zbiorze już istnieje dany kod, zwiększamy zmienną ilości egzemplarzy
                quantities[group_id] = temp;
            }
        }
        void quantityHandlerRemove(Dictionary<string, int> quantities, string group_id)
        {
            try
            {
                int temp = quantities[group_id] - 1;
                quantities[group_id] = temp;
            }
            catch (Exception ex)
            {

            }
        }
    }
}