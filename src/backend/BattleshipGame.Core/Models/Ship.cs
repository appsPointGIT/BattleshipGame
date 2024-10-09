namespace BattleshipGame.Core.Models
{
    public class Ship
    {
        public string Name { get; }
        public int Size { get; }
        public List<Location> Locations { get; }
        public bool IsSunk { get; set; }

        public Ship(string name, int size)
        {
            Name = name;
            Size = size;
            Locations = new();
            IsSunk = false;
        }

    }
}
