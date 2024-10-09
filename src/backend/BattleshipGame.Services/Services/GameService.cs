using BattleshipGame.Core.Enums;
using BattleshipGame.Core.Models;
using BattleshipGame.Services.Extensions;
using BattleshipGame.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BattleshipGame.Services.Services
{
    public class GameService : IGameService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string GridSessionKey = "GameGrid";
        private const string ShipsSessionKey = "GameShips";

        private GameGrid _gameGrid;
        private List<Ship> _ships;

        public GameService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession Session => _httpContextAccessor.HttpContext.Session;

        private void ClearSessionData()
        {
            Session.Remove(GridSessionKey);
            Session.Clear();
        }

        public void StartNewGame()
        {
            ClearSessionData();

            _gameGrid = new GameGrid();
            _ships = new List<Ship>();

            Ship battleship = new Ship("Battleship", 5);
            Ship destroyer1 = new Ship("Destroyer1", 4);
            Ship destroyer2 = new Ship("Destroyer2", 4);

            void PlaceShipRandomly(Ship ship)
            {
                var random = new Random();
                bool placed = false;

                while (!placed)
                {
                    int startX = random.Next(0, GameGrid.GridSizeX);
                    int startY = random.Next(0, GameGrid.GridSizeY);
                    bool isHorizontal = random.Next(0, 2) == 0;

                    if (_gameGrid.IsValidPlacement(ship, startX, startY, isHorizontal))
                    {
                        _gameGrid.PlaceShip(ship, startX, startY, isHorizontal);
                        placed = true;

                        Console.WriteLine($"{ship.Name} placed at X:{startX}, Y:{startY}, horizontal: {isHorizontal}");
                    }
                }
            }

            PlaceShipRandomly(battleship);
            PlaceShipRandomly(destroyer1);
            PlaceShipRandomly(destroyer2);

            _ships.AddRange(new[] { battleship, destroyer1, destroyer2 });

            Session.SetJsonObject(GridSessionKey, _gameGrid);
        }

        public GameGrid GetGameData()
        {
            var _gameGrid = Session.GetJsonObject<GameGrid>(GridSessionKey);

            if (_gameGrid is null) return new();

            _gameGrid.DisplayGrid();

            return _gameGrid;
        }

        public string Attack(int x, int y)
        {
            if (x < 0 || y < 0 || x >= GameGrid.GridSizeX || y >= GameGrid.GridSizeY)
                return "Invalid coordinates.";

            var _gameGrid = Session.GetJsonObject<GameGrid>(GridSessionKey);

            if (_gameGrid is null) return "Start a new game";
            if (_gameGrid.GameResult != GameResult.Playing) return "Game over";
            if (_gameGrid.Grid[y][x] != ShotResult.Empty) return "Already attacked this location.";

            _gameGrid.Hits++;
            var hitShip = _gameGrid.Ships.FirstOrDefault(ship =>
                                    ship.Locations.Any(location => location.X == x && location.Y == y));

            string result = "Missed!";
            if (hitShip != null)
            {
                _gameGrid.Grid[y][x] = ShotResult.Hit;
                result = "Hit!";

                bool allHit = hitShip.Locations.All(location =>
                _gameGrid.Grid[location.Y][location.X] == ShotResult.Hit);

                if (allHit)
                {
                    hitShip.IsSunk = true;
                    result = $"{hitShip.Name} sunk!";
                }
            }
            else
            {
                _gameGrid.Grid[y][x] = ShotResult.Missed;
            }

            Session.SetJsonObject(GridSessionKey, _gameGrid);
            return result;
        }

    }
}
