using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteka
{
    class Customer
    {
        static int nextId;                 //id referencyjne do stosowania auto inkrementacji
        public int id { get; set; }                         //id klienta
        public string name { get; set; }
        [JsonIgnore]
        public List<Resource> borrowedResources { get; set; }

        public Customer(string name)
        {
            this.id = Interlocked.Increment(ref nextId);
            this.name = name;
            borrowedResources = new List<Resource>();
        }

        public string GetName()
        {
            return name;
        }

        public void removeResource(Resource r)
        {
            borrowedResources.Remove(r);
        }

        public void addResource(Resource r)
        {
            borrowedResources.Add(r);
        }

        public override string ToString()
        {
            return "Klient: " + id + "\nLogin: " + name + "\nWypożyczone zasoby: " + borrowedResources.Count() + "\n";
        }
    }
}
