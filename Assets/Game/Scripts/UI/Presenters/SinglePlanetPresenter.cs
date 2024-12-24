using System;
using Modules.Planets;
using Modules.UI;

namespace Game.Game.Scripts.UI.Presenters
{
    public class SinglePlanetPresenter :IDisposable
    {
        private readonly MoneyView _moneyView;
        private readonly ParticleAnimator _particleAnimator;

        private IPlanet _planet;
        private IPlanetView _view;
        public event Action<IPlanet> OnOpenPopupRequest;

        public SinglePlanetPresenter(MoneyView moneyView, ParticleAnimator particleAnimator)
        {
            _moneyView = moneyView;
            _particleAnimator = particleAnimator;
        }
        public void BindPlanetView(IPlanet planet, IPlanetView view)
        {
            _planet = planet;
            _view = view;

            _planet.OnUnlocked += HandleOnPlanetUnlocked;
            _planet.OnUpgraded += HandlePlanetOnUpgraded;
            _planet.OnIncomeReady += HandleOnPlanetIncomeReady;

            _planet.OnIncomeTimeChanged += OnPlanetOnOnIncomeTimeChanged;
            _view.OnHold += OnViewOnOnHold;
            _view.OnClick += OnViewOnOnClick;

            Unlocked(_planet, _view);
            HandleOnUpgraded(_planet, _view);
            IncomeReady(_planet, _view);
        }

        private void OnViewOnOnClick()
        {
            HandleOnPlanetClick(_planet, _view);
        }

        private void OnViewOnOnHold()
        {
            OnOpenPopupRequest?.Invoke(_planet);
        }

        private void OnPlanetOnOnIncomeTimeChanged(float timeLeft)
        {
            IncomeTimeChanged(_planet, _view, timeLeft);
        }

        private void HandleOnPlanetIncomeReady(bool x)
        {
            IncomeReady(_planet, _view);
        }

        private void HandlePlanetOnUpgraded(int x)
        {
            HandleOnUpgraded(_planet, _view);
        }

        private void HandleOnPlanetUnlocked()
        {
            Unlocked(_planet, _view);
        }

        private void IncomeTimeChanged(IPlanet planet, IPlanetView view, float timeLeft)
        {
            view.SetProgress(planet.IncomeProgress);
            var timeString = $"{timeLeft:F0}s";
            view.SetTimer(timeString);
        }

        private void HandleOnPlanetClick(IPlanet planet, IPlanetView planetView)
        {
            if (planet.CanUnlock)
                planet.Unlock();
            else if (planet.IsIncomeReady && planet.IsUnlocked)
            {
                _particleAnimator.Emit(planetView.CoinProjectileAnimationAnchor.position,
                    _moneyView.FlyAnchor.position, 0.5f, _moneyView.Animate);
                planet.GatherIncome();
            }
        }

        private void IncomeReady(IPlanet planet, IPlanetView view)
        {
            view.SetReady(planet.IsIncomeReady);
        }

        private void HandleOnUpgraded(IPlanet planet, IPlanetView view)
        {
            view.SetPrice(planet.Price.ToString());
        }

        private void Unlocked(IPlanet planet, IPlanetView view)
        {
            view.SetLocked(!planet.IsUnlocked);
            view.SetIcon(planet.GetIcon(planet.IsUnlocked));
        }

        public void Dispose()
        {
            _planet.OnUnlocked -= HandleOnPlanetUnlocked;
            _planet.OnUpgraded -= HandlePlanetOnUpgraded;
            _planet.OnIncomeReady -= HandleOnPlanetIncomeReady;

            _planet.OnIncomeTimeChanged -= OnPlanetOnOnIncomeTimeChanged;
            _view.OnHold -= OnViewOnOnHold;
            _view.OnClick -= OnViewOnOnClick;
        }
    }
}