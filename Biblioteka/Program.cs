using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Biblioteka
{
    internal class Biblioteka
    {
        List<Zasoby> zasoby = new List<Zasoby>();                                            //zmienna przechowująca wszystkie zasoby
        List<Customer> customers = new List<Customer>();                                       //zmienna przechowująca wszystkich klientów
        Dictionary<string, int> quantities = new Dictionary<string, int>();         //zmienna przechowująca ilości poszczególnych egzemplarzy
        public interface IBiblioteka
        {
            static int last_resource_id;
            int id { get; set; }         //id konkretnego egzemplarza
            string group_id { get; set; }           //id egzemplarza - kod kreskowy
            string name { get; set; }            //nazwa egzemplarza
            bool isBorrowed { get; set; }       //czy przedmiot mozna wypozyczyc?
            string customer_name { get; set; }  //kto wypożyczył zasób
            bool isRemoved { get; set; }         //czy jest usunięty z bazy danych?
        }
        public class Zasoby : IBiblioteka
        {
            private static int last_resource_id;
            public int id { get; set; }         //id konkretnego egzemplarza
            public string group_id { get; set; }           //id egzemplarza - kod kreskowy
            public string name { get; set; }            //nazwa egzemplarza
            public bool isBorrowed { get; set; }       //czy przedmiot mozna wypozyczyc?
            public string customer_name { get; set; }  //kto wypożyczył zasób
            public bool isRemoved { get; set; }         //czy jest usunięty z bazy danych?

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
            var program = new Biblioteka();
            
            
            program.printMainMenu();
        }

        void printMainMenu()
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
                case 1: rentResource(); printMainMenu(); break;
                case 2: addResource(); printMainMenu(); break;
                case 3: removeResource(); printMainMenu(); break;
                case 4: rentResource(); printMainMenu(); break;
                case 5: printResources(); printMainMenu(); break;
                case 6: writeFile(); printMainMenu(); break;
                case 7: readFile(); printMainMenu(); break;
                default: Console.WriteLine("Niewłaściwy wybór."); printMainMenu(); break;
            }
        }
        void rentResource()
        {
            Console.WriteLine("===================WYPOŻYCZENIE/ZWROT ZASOBU===================");
            Console.WriteLine("1. Nowy klient.");
            Console.WriteLine("2. Powracający klient.");
            int n = Convert.ToInt32(Console.ReadLine());
            switch (n)
            {
                case 1: AddCustomer(); break;
                case 2: login(); break;
                default: Console.WriteLine("Nieprawidłowy wybór."); rentResource(); break;
            }
        }
        void AddCustomer()
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
                    AddCustomer();
                }
            }

            customers.Add(new Customer(name));
            Console.WriteLine("\nRejestracja pomyślna.");
            updateResource(name);
        }
        void login()
        {
            bool logged = false;
            Console.WriteLine("Podaj login.");
            string name = Console.ReadLine();
            foreach (Customer customer in customers)
            {
                if(customer.name == name)
                {
                    Console.WriteLine("Zalogowano\n");
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
                    updateResource(name);
                } else if(n == 2)
                {
                    returnResource(name);
                }
                else
                {
                    Console.WriteLine("Niepoprawny wybór.");
                    login();
                }
            } 
            else
            {
                Console.WriteLine("Niepoprawny login.");
                Console.WriteLine("Naciśnij dowolny przycisk.");
                Console.ReadKey();
                login();
            }
        }
        void updateResource(string name)
        {
            bool borrowed = false;
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
                if (z.id == n && z.isBorrowed == false && z.isRemoved == false)
                {
                    z.isBorrowed = true;
                    customer.wypozyczone.Add(z);
                    z.customer_name = customer.name;
                    Console.WriteLine("Wypożyczono zasób.");
                    Console.WriteLine("Naciśnij dowolny przycisk.");
                    Console.ReadKey();
                    borrowed = true;
                    break;
                }
            }
            if (!borrowed)
            {
                Console.WriteLine("Niepoprawny numer ID lub zasób jest już wypożyczony.");
                updateResource(name);
            }
        }
        void returnResource(string name)
        {
            Customer customer = null;
            foreach (Customer c in customers)
            {
                if (c.name == name)
                {
                    customer = c;
                }
            }
            Console.WriteLine("==============TWOJE ZASOBY==============");
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
                    z.customer_name = null;
                    customer.wypozyczone.Remove(z);
                    Console.WriteLine("Zwrócono zasób.");
                    Console.WriteLine("Naciśnij dowolny przycisk.");
                    Console.ReadKey();
                    break;
                }
            }
        }
        void removeResource()
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
                    if (z.isBorrowed)
                    {
                        z.isBorrowed=false;                                     //zabezpieczenie na wypadek usuniecia zasobu, który jest wypożyczony
                        string name = z.customer_name;
                        Customer c1 = null;
                        z.customer_name=null;
                        foreach(Customer c2 in customers)
                        {
                            if(c2.name == name)
                            {
                                c1 = c2;
                            }
                        }
                        c1.wypozyczone.Remove(z);
                    }
                    group_id = z.group_id;
                    z.isRemoved = true;
                    removed = true;
                }
                i++;
            }
            if (removed)
            {
                quantityHandlerRemove(group_id);
            }
            else
            {
                Console.WriteLine("Błąd podczas usuwania.\n Naciśnij dowolny klawisz.");
                Console.ReadKey();
            }
        }
        void printResources()
        {
            Console.Clear();
            foreach (Zasoby z in zasoby)
            {
                if (z.isRemoved)
                {
                    continue;
                }
                int q;
                quantities.TryGetValue(z.group_id, out q);
                Console.WriteLine("===================================");
                Console.Write("ID: " + z.id + "\nKod Kreskowy: " + z.group_id + "\nNazwa: " + z.name + "\nCzy wypożyczone? " + z.isBorrowed + "\nIlość egzemplarzy: " + q);
                if (z.isBorrowed)
                {
                    Console.WriteLine();
                    Console.WriteLine("Wypożyczający: " + z.customer_name);
                }
                else
                {
                    Console.WriteLine();
                }
                Console.WriteLine("===================================");
            }
            Console.WriteLine();
            Console.WriteLine("Naciśnij dowolny klawisz.");
            Console.ReadKey();
        }
        void addResource()
        {
            Console.Clear();
            Console.WriteLine("===================DODAWANIE ZASOBU===================");
            int choice;
            Console.WriteLine("1.Książka");
            Console.WriteLine("2.Czasopismo");
            Console.WriteLine("3.Film");
            Console.WriteLine("4.Praca naukowa");
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)                                                                     //wybór typu zasobu przez użytkownika
            {
                case 1: addBook(); break;
                case 2: addNewspaper(); break;
                case 3: addMovie(); break;
                case 4: addScientificWork(); break;
                default: Console.WriteLine("Niewłaściwy wybór."); addResource(); break;
            }
        }
        void addBook()
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
            quantityHandler(n);
        }
        void addNewspaper()
        {
            string s, d, n;
            Console.WriteLine("Podaj nazwę:");
            s = Console.ReadLine();
            Console.WriteLine("Podaj kod kreskowy:");
            n = Console.ReadLine();
            Console.WriteLine("Podaj date wydania w formacie DD.MM.YYYY:");
            d = Console.ReadLine();
            zasoby.Add(new Newspaper(s, n, d));
            quantityHandler(n);
        }
        void addMovie()
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
            quantityHandler(n);
        }
        void addScientificWork()
        {
            string s, a, n;
            Console.WriteLine("Podaj nazwę:");
            s = Console.ReadLine();
            Console.WriteLine("Podaj kod kreskowy:");
            n = Console.ReadLine();
            Console.WriteLine("Podaj autora:");
            a = Console.ReadLine();
            zasoby.Add(new ScientificWork(s, n, a));
            quantityHandler(n);
        }
        void quantityHandler(string group_id)
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
        void quantityHandlerRemove(string group_id)
        {
            try
            {
                int temp = quantities[group_id] - 1;            // szukamy kodu kreskowego i zmniejszamy ilość egzemplarzy
                quantities[group_id] = temp;
            }
            catch (Exception ex)
            {

            }
        }
        void readFile()                                                     //zapis stanu
        {
            string fileName = "zasoby.json";
            string jsonString = File.ReadAllText(fileName);
            zasoby = JsonConvert.DeserializeObject<List<Zasoby>>(jsonString);
            
            fileName = "customers.json";
            jsonString = File.ReadAllText(fileName);
            customers = JsonConvert.DeserializeObject<List<Customer>>(jsonString);


            fileName = "quantities.json";
            jsonString = File.ReadAllText(fileName);
            quantities = JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString);
        }
        void writeFile()                                                    //odczyt stanu
        {
            string JSONresult = JsonConvert.SerializeObject(zasoby, Formatting.Indented);
            string path = "zasoby.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JSONresult);
            JSONresult = JsonConvert.SerializeObject(customers, Formatting.Indented); ;
            path = "customers.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JSONresult);
            JSONresult = JsonConvert.SerializeObject(quantities, Formatting.Indented);
            path = "quantities.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JSONresult);
        }
    }
}