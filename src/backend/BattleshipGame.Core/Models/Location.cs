namespace BattleshipGame.Core.Models
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsHorizontal { get; set; } = true;

        public Location(int x, int y, bool isHorizontal = true)
        {
            X = x;
            Y = y;
            IsHorizontal = isHorizontal;
        }
    }
}
