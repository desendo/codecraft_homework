using System.Collections.Generic;
using System.Linq;
using Game;
using Game.Game.Scripts.Pools;
using Game.Game.Scripts.UI.Presenters;
using Game.Gameplay;
using Modules.Planets;
using UnityEngine;
using Zenject;

namespace Gameplay
{
    public sealed class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private int _initialMoney = 300;

        [SerializeField] private PlanetCatalog _catalog;

        [SerializeField] private PlanetPopupView _planetPopupView;
        [SerializeField] private List<PlanetView> _planetViews;
        [SerializeField] private MoneyView _moneyView;
        [SerializeField] private CoinView _coinView;
        [SerializeField] private RectTransform _projectileParent;

        public override void InstallBindings()
        {
            MoneyInstaller.Install(this.Container, _initialMoney);
            PlanetInstaller.Install(this.Container, _catalog);

            //views
            Container.Bind<MoneyView>().FromInstance(_moneyView).AsSingle().NonLazy();
            Container.Bind<PlanetPopupView>().FromInstance(_planetPopupView).AsSingle().NonLazy();
            Container.Bind<List<IPlanetView>>().FromInstance(_planetViews.Cast<IPlanetView>().ToList()).AsSingle().NonLazy();

            //pool
            Container.BindMemoryPool<CoinView, CoinPool>().FromComponentInNewPrefab(_coinView).UnderTransform(_projectileParent);
            Container.Bind<ICoinsSpawner>().To<CoinPool>().FromResolve();

            //presenters
            Container.Bind<MoneyPresenter>().AsSingle().NonLazy();
            Container.Bind<PlanetPopupPresenter>().AsSingle().NonLazy();
            Container.Bind<PlanetsPresenter>().AsSingle().NonLazy();


        }
    }
}