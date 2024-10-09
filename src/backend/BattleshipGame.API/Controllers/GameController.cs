using BattleshipGame.Core.DTOs;
using BattleshipGame.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly ILogger<GameController> _logger;

        public GameController(IGameService gameService, ILogger<GameController> logger)
        {
            _gameService = gameService;
            _logger = logger;
        }

        [HttpGet("start")]
        public IActionResult StartGame()
        {
            try
            {
                _gameService.StartNewGame();
                return Ok("New game started");
            }
            catch (Exception ex)
            {
                string msg = "An error occurred while starting new game.";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            try
            {
                var grid = _gameService.GetGameData();
                return Ok(grid);
            }
            catch (Exception ex)
            {
                string msg = "An error occurred while retriving the status.";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

        [HttpPost("attack")]
        public IActionResult Attack([FromBody] AttackRequest request)
        {
            try
            {
                var result = _gameService.Attack(request.X, request.Y);
                return Ok(new { Result = result });
            }
            catch (Exception ex)
            {
                string msg = "An error occurred while attacking.";
                _logger.LogError(ex, msg);
                return StatusCode(500, msg);
            }
        }

    }
}
