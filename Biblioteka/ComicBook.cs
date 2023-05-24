namespace Biblioteka
{
    internal class ComicBook : Resource
    {
        public string Universe { get; set; }

        public ComicBook(string barcode, string name, string description, Location location, string dateOfIssue, string universe)
            : base(barcode, name, description, dateOfIssue, location)
        {
            Universe = universe;
        }

        public override string ToString()
        {
            if (IsDamaged)
            {
                return $"Komiks: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nLokalizacja w bibliotece: {Location}\nRok wydania: {DateOfIssue}\nUniwersum: {Universe}\nUszkodzony - niedostępny do wypożyczenia";
            }
            if (Customer == null)
            {
                return $"Komiks: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nLokalizacja w bibliotece: {Location}\nRok wydania: {DateOfIssue}\nUniwersum: {Universe}\nDostępna do wypożyczenia";
            }

            return $"Komiks: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nRok wydania: {DateOfIssue}\nUniwersum: {Universe}\nWypożyczający: {Customer.GetName()}";
        }
    }
}
