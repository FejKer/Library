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
            public int group_id { get; set; }           //id egzemplarza - kod kreskowy
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
            private int pages;
            private string author;
            public Book(string name, int group_id)
            {
                this.name = name;
                this.group_id = group_id;
            }
        }

        public class Newspaper : Zasoby
        {
            private int date;
            public Newspaper()
            {

            }
        }

        public class Movie : Zasoby
        {
            private int length;
            public Movie()
            {

            }
        }

        public class ScientificWork : Zasoby
        {
            private string author;
            public ScientificWork()
            {

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
                this.wypozyczone = new List<Zasoby>();                                 //wypozyczone zaosby
            }
        }

        static void Main(string[] args)
        {
            var program = new Program();
            /*
            Console.WriteLine("===================Biblioteka===================");
            Console.WriteLine("1.Wypożycz przedmiot.");
            Console.WriteLine("2.Dodaj nowy przedmiot.");
            Console.WriteLine("3.Usuń przedmiot.");
            Console.WriteLine("4.Zwróć przedmiot.");
            Console.WriteLine("5.Wyświetl zasoby.");
            Console.WriteLine("6.Zapisz stan do pliku.");
            Console.WriteLine("7.Wczytaj stan z pliku.");
            
                        int n = Convert.ToInt32(Console.ReadLine()); //czytamy wybor uzytkownika

                        switch (n)
                        {
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                            default: Console.WriteLine("Niewłaściwy wybór."); break;
                        }
            */
            var zasoby = new List<Zasoby>();                //zmienna przechowująca wszystkie zasoby
            var customers = new List<Customer>();           //zmienna przechowująca wszystkich klientów
            program.addResource(zasoby);
            program.printResources(zasoby);
            System.Threading.Thread.Sleep(2000);
            program.addResource(zasoby);
            program.printResources(zasoby);
            System.Threading.Thread.Sleep(2000);
            program.removeResource(zasoby);
            program.printResources(zasoby);
            System.Threading.Thread.Sleep(5000);
        }

        void removeResource(dynamic zasoby)
        {
            Console.Clear();
            int id_to_remove;
            Console.WriteLine("===================USUWANIE ZASOBU===================");
            Console.WriteLine("Podaj id zasobu do usunięcia");
            id_to_remove = Convert.ToInt32(Console.ReadLine());
            int i = 0;
            foreach (Zasoby z in zasoby.ToArray())
            {
                if (z.id == id_to_remove)
                {
                    zasoby.RemoveAt(i);
                }
                i++;
            }
        }
        void printResources(dynamic zasoby)
        {
            Console.Clear();
            foreach (Zasoby z in zasoby)
            {
                Console.WriteLine("===================================");
                Console.WriteLine("ID: " + z.id + " Kod Kreskowy: " + z.group_id + " Nazwa: " + z.name + " Czy wypożyczone? " + z.isAvailable);
                Console.WriteLine("===================================");
            }
        }
        void addResource(dynamic zasoby)
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
                case 1: addBook(zasoby); break;
                case 2: addNewspaper(zasoby); break;
                case 3: addMovie(zasoby); break;
                case 4: addScientificWork(zasoby); break;
                default: Console.WriteLine("Niewłaściwy wybór."); addResource(zasoby); break;
            }
        }
        void addBook(dynamic zasoby)
        {
            int n;
            string s;
            Console.WriteLine("Podaj nazwę:");
            s = Console.ReadLine();
            Console.WriteLine("Podaj kod kreskowy:");
            n = Convert.ToInt32(Console.ReadLine());
            zasoby.Add(new Book(s, n));
        }
        void addNewspaper(dynamic zasoby)
        {

        }
        void addMovie(dynamic zasoby)
        {

        }
        void addScientificWork(dynamic zasoby)
        {

        }
    }
}
