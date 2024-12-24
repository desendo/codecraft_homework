using System;
using System.Collections.Generic;
using Modules.Planets;
using Modules.UI;
using Zenject;

namespace Game.Game.Scripts.UI.Presenters
{
    public class PlanetsPresenter : IDisposable
    {
        private readonly PlanetPopupPresenter _planetPopupPresenter;

        private readonly List<SinglePlanetPresenter> _planetPresenters = new List<SinglePlanetPresenter>();
        public PlanetsPresenter(List<IPlanet> planets, List<IPlanetView> planetViews,
            PlanetPopupPresenter planetPopupPresenter, DiContainer container)
        {
            if (planets.Count != planetViews.Count)
                throw new Exception("different views and models count");


            for (var index = 0; index < planets.Count; index++)
            {
                var planetPresenter = container.Instantiate<SinglePlanetPresenter>();

                planetPresenter.OnOpenPopupRequest+= HandleOnOpenPopupRequest;
                planetPresenter.BindPlanetView(planets[index], planetViews[index]);

                _planetPresenters.Add(planetPresenter);
            }

            _planetPopupPresenter = planetPopupPresenter;
        }


        private void HandleOnOpenPopupRequest(IPlanet obj)
        {
            _planetPopupPresenter.OpenPopup(obj);
        }

        public void Dispose()
        {
            foreach (var singlePlanetPresenter in _planetPresenters)
            {
                singlePlanetPresenter.OnOpenPopupRequest -= HandleOnOpenPopupRequest;
                singlePlanetPresenter.Dispose();
            }
        }
    }
}