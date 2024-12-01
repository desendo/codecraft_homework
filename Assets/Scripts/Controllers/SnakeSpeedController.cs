using Modules;

namespace Game.Controllers
{
    public class SnakeSpeedController
    {
        public SnakeSpeedController(IDifficulty difficulty, ISnake snake)
        {
            difficulty.OnStateChanged += () => snake.SetSpeed(difficulty.Current);
        }
    }
}