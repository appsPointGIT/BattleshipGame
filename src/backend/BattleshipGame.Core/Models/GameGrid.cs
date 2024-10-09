using BattleshipGame.Core.Enums;

namespace BattleshipGame.Core.Models
{
    public class GameGrid
    {
        public const int GridSizeX = 10;
        public const int GridSizeY = 10;
        public const int MaxHittingCount = 20;

        public ShotResult[][] Grid { get; set; }
        public List<Ship> Ships { get; set; } = new();
        public int Hits { get; set; } = 0;
        public int RemainingHits => Math.Max(0, MaxHittingCount - Hits);
        public GameResult GameResult => Ships.All(x => x.IsSunk == true)
                                            ? GameResult.Won
                                            : ((RemainingHits == 0) ? GameResult.Lost : GameResult.Playing);


        public GameGrid()
        {
            Grid = new ShotResult[GridSizeY][];
            for (int i = 0; i < GridSizeY; i++)
            {
                Grid[i] = new ShotResult[GridSizeX];
            }
        }

        public bool IsValidPlacement(Ship ship, int startX, int startY, bool isHorizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? startX + i : startX;
                int y = isHorizontal ? startY : startY + i;

                if (x >= GridSizeX || y >= GridSizeY)
                    return false;

                if (Ships.Any(s => s.Locations.Any(loc => loc.X == x && loc.Y == y)))
                    return false;
            }

            return true;
        }

        public void PlaceShip(Ship ship, int startX, int startY, bool isHorizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int x = isHorizontal ? startX + i : startX;
                int y = isHorizontal ? startY : startY + i;

                ship.Locations.Add(new Location(x, y, isHorizontal));
            }
            Ships.Add(ship);
        }

        public void DisplayGrid()
        {
            Console.WriteLine("---------------------------------");
            for (int i = 0; i < GridSizeY; i++)
            {
                for (int j = 0; j < GridSizeX; j++)
                {
                    switch (Grid[i][j])
                    {
                        case ShotResult.Empty:
                            Console.Write(" ~ ");
                            break;
                        case ShotResult.Hit:
                            Console.Write(" X ");
                            break;
                        case ShotResult.Missed:
                            Console.Write(" O ");
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine("---------------------------------");
            Console.WriteLine();
        }

    }
}
