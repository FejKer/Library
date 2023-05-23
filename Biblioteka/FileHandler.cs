using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Biblioteka
{
    internal class FileHandler
    {

        public static void ReadFile()                                                     //odczyt stanu
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            string fileName = "resources.json";
            string jsonString = File.ReadAllText(fileName);
            Database.SetResources(JsonConvert.DeserializeObject<List<Resource>>(jsonString, settings));

            fileName = "customers.json";
            jsonString = File.ReadAllText(fileName);
            Database.SetCustomers(JsonConvert.DeserializeObject<List<Customer>>(jsonString, settings));


            fileName = "quantities.json";
            jsonString = File.ReadAllText(fileName);
            Database.SetQuantities(JsonConvert.DeserializeObject<Dictionary<string, int>>(jsonString, settings));

            foreach (var r in Database.GetResources())
            {
                foreach (var c in Database.GetCustomers())
                {
                    if (r.Customer != null && r.Customer.Name == c.Name)
                    {
                        c.AddResource(r);
                        break;
                    }
                }
            }
        }
        public static void WriteFile()                                                    //zapis stanu
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
            string JSONresult = JsonConvert.SerializeObject(Database.GetResources(), Formatting.Indented, settings);
            string path = "resources.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JSONresult);
            Console.WriteLine(JSONresult);
            JSONresult = JsonConvert.SerializeObject(Database.GetCustomers(), Formatting.Indented, settings);
            path = "customers.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JSONresult);
            Console.WriteLine(JSONresult);
            JSONresult = JsonConvert.SerializeObject(Database.GetQuantities(), Formatting.Indented, settings);
            path = "quantities.json";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            File.WriteAllText(path, JSONresult);
            Console.WriteLine(JSONresult);
        }
    }
}