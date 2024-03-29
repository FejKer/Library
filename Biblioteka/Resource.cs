﻿using System.Collections.Generic;
using System.Threading;

namespace Biblioteka
{
    internal abstract class Resource
    {
        private static int nextId = 0;

        public int Id { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DateOfIssue { get; set; }
        public Location Location { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDamaged { get; set; }
        public Customer? Customer { get; set; }

        protected Resource(string barcode, string name, string description, string dateOfIssue, Location location)
        {
            Id = Interlocked.Increment(ref nextId);
            Barcode = barcode;
            Name = name;
            Description = description;
            DateOfIssue = dateOfIssue;
            Location = location;
            IsAvailable = true;
            IsDeleted = false;
            IsDamaged = false;
        }

        public static void RentResource(Customer customer, Resource resource)
        {
            if (!resource.IsAvailable || resource.IsDeleted || resource.IsDamaged)
            {
                MenuHelper.PrintMenu("Błąd przy wypożyczaniu");
                return;
            }

            resource.IsAvailable = false;
            resource.Customer = customer;
            customer.AddResource(resource);
        }

        public static void ReturnResource(Customer customer, Resource resource)
        {
            if (customer != null)
            {
                customer.RemoveResource(resource);
                resource.Customer = null;
                resource.IsAvailable = true;
            }
        }

        public string GetBarcode()
        {
            return Barcode;
        }

        public override string ToString()
        {
            if (IsDamaged)
            {
                return $"Zasób: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nOpis: {Description}\nLokalizacja w bibliotece: {Location}\nRok wydania: {DateOfIssue}\nUszkodzony - niedostępny do wypożyczenia";
            }
            if (Customer == null)
            {
                return $"Zasób: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nOpis: {Description}\nLokalizacja w bibliotece: {Location}\nRok wydania: {DateOfIssue}\nDostępny do wypożyczenia";
            }

            return $"Zasób: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nOpis: {Description}\nRok wydania: {DateOfIssue}\nWypożyczający: {Customer.GetName()}";
        }
    }
}
