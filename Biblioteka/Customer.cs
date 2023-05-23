using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Biblioteka
{
    internal class Customer
    {
        private static int nextId = 0;

        public int Id { get; set; }
        public string Name { get; set; }
        public List<Resource> BorrowedResources { get; set; }

        public Customer(string name)
        {
            Id = Interlocked.Increment(ref nextId);
            Name = name;
            BorrowedResources = new List<Resource>();
        }

        public string GetName()
        {
            return Name;
        }

        public void RemoveResource(Resource r)
        {
            BorrowedResources.Remove(r);
        }

        public void AddResource(Resource r)
        {
            BorrowedResources.Add(r);
        }

        public override string ToString()
        {
            return $"Klient: {Id}\nLogin: {Name}\nWypożyczone zasoby: {BorrowedResources.Count}\n";
        }
    }
}
