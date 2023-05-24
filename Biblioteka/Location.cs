namespace Biblioteka
{
    public class Location
    {
        public Location(int floor, string alley)
        {
            Floor = floor;
            Alley = alley;
        }

        public int Floor { get; set; }
        public string Alley { get; set; }

        public override string ToString()
        {
            return "\n   Piętro: " + Floor + "\n   Alejka: " + Alley;
        }
    }
}