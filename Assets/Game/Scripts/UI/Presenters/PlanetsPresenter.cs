using System;
using System.Collections.Generic;
using Game.Game.Scripts.Pools;
using Modules.Planets;
using UnityEngine;

namespace Game.Game.Scripts.UI.Presenters
{
    public class PlanetsPresenter
    {
        private readonly PlanetPopupPresenter _planetPopupPresenter;
        private readonly MoneyView _moneyView;
        private readonly ICoinsSpawner _coinsSpawner;

        public PlanetsPresenter(List<IPlanet> planets, List<IPlanetView> planetViews,
            PlanetPopupPresenter planetPopupPresenter, MoneyView moneyView, ICoinsSpawner coinsSpawner)
        {
            if (planets.Count != planetViews.Count)
                throw new Exception("different views and models count");


            for (var index = 0; index < planets.Count; index++)
            {
                BindPlanetView(planets[index], planetViews[index]);
            }

            _planetPopupPresenter = planetPopupPresenter;
            _moneyView = moneyView;
            _coinsSpawner = coinsSpawner;
        }

        private void BindPlanetView(IPlanet planet, IPlanetView view)
        {
            planet.OnUnlocked += () => HandleOnUnlocked(planet, view);
            planet.OnUpgraded += x => HandleOnUpgraded(planet, view);
            planet.OnIncomeReady += x => HandleOnIncomeReady(planet, view);
            planet.OnIncomeReady += x => HandleOnIncomeReady(planet, view);

            planet.OnIncomeTimeChanged += timeLeft => HandleOnIncomeTimeChanged(planet, view, timeLeft);

            HandleOnUnlocked(planet, view);
            HandleOnUpgraded(planet, view);
            HandleOnIncomeReady(planet, view);

            view.OnHold += () => _planetPopupPresenter.OpenPopup(planet);
            view.OnClick += () => HandleOnPlanetClick(planet, view);
        }

        private void HandleOnIncomeTimeChanged(IPlanet planet, IPlanetView view, float timeLeft)
        {
            view.SetProgress(planet.IncomeProgress);
            var timeString = $"{timeLeft:F0}s";
            view.SetTimer(timeString);
        }

        private void HandleOnPlanetClick(IPlanet planet, IPlanetView planetView)
        {
            if (planet.CanUnlock)
                planet.Unlock();
            else if (planet.IsIncomeReady)
            {
                planet.GatherIncome();
                FlyCoin(planetView.CoinProjectileAnimationAnchor);
            }
        }

        private void FlyCoin(Transform start)
        {
            _coinsSpawner.Spawn(start.position, _moneyView.FlyAnchor.position);
        }

        private void HandleOnIncomeReady(IPlanet planet, IPlanetView view)
        {
            view.SetReady(planet.IsIncomeReady);
        }

        private void HandleOnUpgraded(IPlanet planet, IPlanetView view)
        {
            view.SetPrice(planet.Price.ToString());
        }

        private void HandleOnUnlocked(IPlanet planet, IPlanetView view)
        {
            view.SetLocked(!planet.IsUnlocked);
            view.SetIcon(planet.GetIcon(planet.IsUnlocked));
        }
    }
}