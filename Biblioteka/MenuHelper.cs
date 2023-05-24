using System;

namespace Biblioteka
{
    internal static class MenuHelper
    {
        public static void PrintMenu(string message)
        {
            Console.Clear();
            Console.WriteLine("====================Biblioteka====================");
            Console.WriteLine("1. Dodaj zasób");
            Console.WriteLine("2. Dodaj klienta");
            Console.WriteLine("3. Wypisz zasoby");
            Console.WriteLine("4. Wypisz klientów");
            Console.WriteLine("5. Wypożycz zasób");
            Console.WriteLine("6. Modyfikuj zasoby biblioteki");
            Console.WriteLine("7. Zwróć wypożyczony zasób");
            Console.WriteLine("8. Zapisz stan bazy do pliku");
            Console.WriteLine("9. Odczytaj stan bazy z pliku");
            Console.WriteLine("==================================================");
            Console.WriteLine("\n" + message);

            int choice;
            bool isValidChoice = int.TryParse(Console.ReadLine(), out choice);

            if (!isValidChoice)
            {
                PrintMenu("Nieprawidłowy wybór");
                return;
            }

            switch (choice)
            {
                case 1:
                    ResourceHelper.AddResource();
                    break;
                case 2:
                    CustomerHelper.AddCustomer();
                    break;
                case 3:
                    ResourceHelper.PrintResources();
                    break;
                case 4:
                    CustomerHelper.PrintCustomers();
                    break;
                case 5:
                    ResourceHelper.RentResource();
                    break;
                case 6:
                    ResourceHelper.ModifyResource();
                    break;
                case 7:
                    ResourceHelper.ReturnResource();
                    break;
                case 8:
                    FileHandler.WriteFile();
                    PrintMenu("Zapisano stan do pliku");
                    break;
                case 9:
                    FileHandler.ReadFile();
                    PrintMenu("Odczytano stan z pliku");
                    break;
                default:
                    PrintMenu("Nieprawidłowy wybór");
                    break;
            }
        }
    }
}
