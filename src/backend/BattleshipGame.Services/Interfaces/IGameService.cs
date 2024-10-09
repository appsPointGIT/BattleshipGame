using BattleshipGame.Core.Models;

namespace BattleshipGame.Services.Interfaces
{
    public interface IGameService
    {
        void StartNewGame();
        GameGrid GetGameData();
        string Attack(int x, int y);
    }
}
