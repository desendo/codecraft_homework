using Game.StateServices;
using Modules;
using UnityEngine;

namespace Game.Controllers
{
    public class PickCoinController
    {
        private readonly ISnake _snake;
        private readonly ICoinsService _coinsService;
        private readonly IScore _score;

        public PickCoinController(ISnake snake, ICoinsService coinsService, IScore score)
        {
            _snake = snake;
            _coinsService = coinsService;
            _score = score;
            _snake.OnMoved += HandleSnakeMoved;
        }

        private void HandleSnakeMoved(Vector2Int pos)
        {
            if (_coinsService.TryPickCoin(pos, out var score, out var bones))
            {
                _snake.Expand(bones);
                _score.Add(score);
            }
        }
    }
}