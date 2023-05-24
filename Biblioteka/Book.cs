namespace Biblioteka
{
    internal class Book : Resource
    {
        public int Pages { get; set; }

        public Book(string barcode, string name, string description, Location location, string dateOfIssue, int pages)
            : base(barcode, name, description, dateOfIssue, location)
        {
            Pages = pages;
        }

        public override string ToString()
        {
            if (IsDamaged)
            {
                return $"Książka: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nLokalizacja w bibliotece: {Location}\nRok wydania: {DateOfIssue}\nIlość stron: {Pages}\nUszkodzony - niedostępny do wypożyczenia";
            }
            if (Customer == null)
            {
                return $"Książka: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nLokalizacja w bibliotece: {Location}\nRok wydania: {DateOfIssue}\nIlość stron: {Pages}\nDostępna do wypożyczenia";
            }

            return $"Książka: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nRok wydania: {DateOfIssue}\nIlość stron: {Pages}\nWypożyczający: {Customer.GetName()}";
        }
    }
}
