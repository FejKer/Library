namespace Biblioteka
{
    internal class ComicBook : Resource
    {
        public string Universe { get; set; }

        public ComicBook(string barcode, string name, string description, string dateOfIssue, string universe)
            : base(barcode, name, description, dateOfIssue)
        {
            Universe = universe;
        }

        public override string ToString()
        {
            if (Customer == null)
            {
                return $"Komiks: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nRok wydania: {DateOfIssue}\nUniwersum: {Universe}\nDostępna do wypożyczenia";
            }

            return $"Komiks: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nRok wydania: {DateOfIssue}\nUniwersum: {Universe}\nWypożyczający: {Customer.GetName()}";
        }
    }
}
