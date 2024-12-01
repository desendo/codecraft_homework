using Game.StateServices;
using Modules;
using SnakeGame;

namespace Game.Controllers
{
    public class UpdateUIController
    {
        private readonly IScore _score;
        private readonly IGameUI _gameUI;
        private readonly IDifficulty _difficulty;
        private readonly IGameStateService _gameStateService;
        private readonly ICoinsService _coinsService;

        public UpdateUIController(IScore score, IGameUI gameUI, IDifficulty difficulty, IGameStateService gameStateService,
            ICoinsService coinsService)
        {
            _score = score;
            _gameUI = gameUI;
            _difficulty = difficulty;
            _gameStateService = gameStateService;
            _coinsService = coinsService;

            _difficulty.OnStateChanged += UpdateLevel;
            _score.OnStateChanged += (x) => UpdateScore();
            _gameStateService.OnGameState += UpdateGameOverScreen;
            UpdateGameOverScreen();
            UpdateScore();
            UpdateLevel();
        }

        private void UpdateGameOverScreen()
        {
            if(_gameStateService.Current == IGameStateService.GameState.Playing)
                return;

            _gameUI.GameOver(_gameStateService.Current == IGameStateService.GameState.Win);
        }


        private void UpdateLevel()
        {
            _gameUI.SetDifficulty(_difficulty.Current, _difficulty.Max);
        }

        private void UpdateScore()
        {
            _gameUI.SetScore(_score.Current.ToString());

        }
    }
}