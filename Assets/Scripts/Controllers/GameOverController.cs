using Game.StateServices;
using Modules;
using SnakeGame;
using UnityEngine;

namespace Game.Controllers
{

    public class GameOverController
    {
        private readonly IGameStateService _gameStateService;
        private readonly ISnake _snake;
        private readonly IWorldBounds _worldBounds;

        public GameOverController(IGameStateService gameStateService, ISnake snake, IWorldBounds worldBounds)
        {
            _gameStateService = gameStateService;
            _snake = snake;
            _worldBounds = worldBounds;
            snake.OnSelfCollided += GameOverLost;
            snake.OnMoved+= SnakeOnMoved;
        }

        private void GameOverLost()
        {
            _snake.SetActive(false);
            _gameStateService.SetState(IGameStateService.GameState.Lost);
        }

        private void SnakeOnMoved(Vector2Int obj)
        {
            if(!_worldBounds.IsInBounds(obj))
                GameOverLost();
        }
    }
}