namespace Biblioteka
{
    internal class Book : Resource
    {
        public int Pages { get; set; }

        public Book(string barcode, string name, string description, string dateOfIssue, int pages)
            : base(barcode, name, description, dateOfIssue)
        {
            Pages = pages;
        }

        public override string ToString()
        {
            if (Customer == null)
            {
                return $"Książka: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nRok wydania: {DateOfIssue}\nIlość stron: {Pages}\nDostępna do wypożyczenia";
            }

            return $"Książka: {Id}\nTytuł: {Name}\nKod kreskowy: {Barcode}\nAutor: {Description}\nRok wydania: {DateOfIssue}\nIlość stron: {Pages}\nWypożyczający: {Customer.GetName()}";
        }
    }
}
