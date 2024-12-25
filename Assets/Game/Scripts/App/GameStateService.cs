using System;

namespace Game.Game.Scripts.App
{
    public class GameStateService : IGameStateService
    {
        public event Action OnLoading;
        public bool IsLoading { get; private set; }

        public void SetIsLoading(bool isLoading)
        {
            IsLoading = isLoading;
            OnLoading?.Invoke();
        }
    }

    public interface IGameStateService: IReadOnlyGameStateService
    {
        void SetIsLoading(bool isLoading);
    }

    public interface IReadOnlyGameStateService
    {
        public event Action OnLoading;
        public bool IsLoading { get; }
    }
}