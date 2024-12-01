using System;

namespace Game.StateServices
{
    public class GameStateService : IGameStateService
    {
        private IGameStateService.GameState _current;
        public IGameStateService.GameState Current => _current;
        public event Action OnGameState;

        public void SetState(IGameStateService.GameState gameState)
        {
            _current = gameState;
            OnGameState?.Invoke();
        }

    }

    public interface IGameStateService
    {
        public GameState Current { get; }
        event Action OnGameState;
        public enum GameState
        {
            Playing,
            Win,
            Lost
        }

        void SetState(GameState gameState);
    }
}