using Game.Controllers;
using Game.Pools;
using Game.StateServices;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameUI _gameUI;
    [SerializeField] private WorldBounds _worldBounds;
    [SerializeField] private Snake _snake;
    [SerializeField] private Coin _coinPrefab;
    [SerializeField] private Transform _coinsParent;

    const int LevelsMax = 9;

    public override void InstallBindings()
    {
        //install legacy
        Container.BindInterfacesAndSelfTo<IGameUI>().FromInstance(_gameUI).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<IWorldBounds>().FromInstance(_worldBounds).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<ISnake>().FromInstance(_snake).AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Score>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<Difficulty>().AsSingle().WithArguments(LevelsMax).NonLazy();
        Container.BindInterfacesAndSelfTo<Coin>().FromInstance(_coinPrefab).AsSingle().NonLazy();

        //install pool
        Container.BindMemoryPool<Coin, CoinsPool>().FromComponentInNewPrefab(_coinPrefab).UnderTransform(_coinsParent);
        Container.Bind<ICoinsSpawner>().To<CoinsPool>().FromResolve();

        //install services
        Container.BindInterfacesAndSelfTo<GameStateService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CoinsService>().AsSingle().NonLazy();

        //install controllers
        Container.BindInterfacesAndSelfTo<InputController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameOverController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<UpdateUIController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<StartLevelController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<SnakeSpeedController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PickCoinController>().AsSingle().NonLazy();

    }
    public void Initialize()
    {
        var inits = Container.ResolveAll<IInit>();
        foreach (var init in inits)
        {
            init.Initialize();
        }
    }
}