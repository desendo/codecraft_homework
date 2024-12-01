using System;
using Game.StateServices;
using Modules;

namespace Game.Controllers
{
    public class StartLevelController : IInit
    {
        private readonly IDifficulty _difficulty;
        private readonly IGameStateService _gameStateService;
        private readonly ISnake _snake;
        private readonly ICoinsService _coinsService;

        public StartLevelController(IDifficulty difficulty, IGameStateService gameStateService, ISnake snake,
            ICoinsService coinsService)
        {
            _difficulty = difficulty;
            _gameStateService = gameStateService;
            _snake = snake;
            _coinsService = coinsService;
            coinsService.OnCoinsChanged+= HandleOnCoinsChanged;
        }

        public void Initialize()
        {
            TryStartNewLevel();
        }

        private void HandleOnCoinsChanged()
        {
            if (_coinsService.LevelCoinsLeft == 0)
                TryStartNewLevel();
        }

        private void TryStartNewLevel()
        {
            if (_difficulty.Next(out var level))
                _coinsService.GenerateCoins(level);
            else
                GameOverWin();
        }

        private void GameOverWin()
        {
            _snake.SetActive(false);

            _gameStateService.SetState(IGameStateService.GameState.Win);
        }
    }
}