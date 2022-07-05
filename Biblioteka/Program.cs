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
            public int quantity { get; set; }           //ilosc egzemplarzy
            public bool isAvailable { get; set; }       //czy przedmiot mozna wypozyczyc?

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
            public List<Zasoby> wypozyczone;
            public Customer()
            {
                id = Interlocked.Increment(ref last_customer_id);                      //id klienta z automatyczną inkrementacją
                this.wypozyczone = new List<Zasoby>();                                 //wypozyczone zasoby
            }
        }

        static void Main(string[] args)
        {
            var program = new Program();
            var zasoby = new List<Zasoby>();                                            //zmienna przechowująca wszystkie zasoby
            var customers = new List<Customer>();                                       //zmienna przechowująca wszystkich klientów
            Dictionary<string, int> quantities = new Dictionary<string, int>();
            
            program.printMainMenu(zasoby, quantities);

        }

        void printMainMenu(dynamic zasoby, Dictionary<string, int> quantities)
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
            int n = Convert.ToInt32(Console.ReadLine());                                   //czytamy wybor uzytkownika

            switch (n)
            {
                case 1:
                case 2: addResource(zasoby, quantities); printMainMenu(zasoby, quantities); break;
                case 3: removeResource(zasoby, quantities); printMainMenu(zasoby, quantities); break;
                case 4:
                case 5: printResources(zasoby, quantities); printMainMenu(zasoby, quantities); break;
                default: Console.WriteLine("Niewłaściwy wybór."); printMainMenu(zasoby, quantities); break;
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
        }
        void printResources(dynamic zasoby, Dictionary<string, int> quantities)
        {
            Console.Clear();
            foreach (Zasoby z in zasoby)
            {
                int q;
                quantities.TryGetValue(z.group_id, out q);
                Console.WriteLine("===================================");
                Console.WriteLine("ID: " + z.id + "\nKod Kreskowy: " + z.group_id + "\nNazwa: " + z.name + "\nCzy wypożyczone? " + z.isAvailable + "\nIlość egzemplarzy: " + q);
                Console.WriteLine("===================================");
            }
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
                quantities.Add(group_id, 1);
            }
            catch (Exception ex)
            {
                int temp = quantities[group_id] + 1;
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