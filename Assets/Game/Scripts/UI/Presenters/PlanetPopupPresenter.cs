using System;
using Modules.Money;
using Modules.Planets;
using UnityEngine;

namespace Game.Game.Scripts.UI.Presenters
{
    public class PlanetPopupPresenter : IDisposable
    {
        private readonly PlanetPopupView _planetPopupView;
        private IPlanet _selectedPlanet;
        private readonly IMoneyStorage _moneyStorage;

        public bool IsUpgradeEnabled { get; private set; }
        public string PopulationText { get; private set; }
        public string IncomeText { get; private set; }
        public string LevelText { get; private set; }
        public string TitleText { get; private set; }
        public string UpgradeCostText { get; private set; }
        public Sprite Icon { get; private set; }
        public bool IsMaxLevel { get; set; }

        public event Action StateChanged;

        public PlanetPopupPresenter(PlanetPopupView planetPopupView, IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
            _planetPopupView = planetPopupView;
            _moneyStorage.OnMoneyChanged += HandleOnMoneyChanged;
        }


        public void OpenPopup(IPlanet planet)
        {
            if(!planet.IsUnlocked)
                return;

            _planetPopupView.gameObject.SetActive(true);
            BindPlanetPopup(planet);
        }

        private void HandleOnMoneyChanged(int x, int y)
        {
            UpdatePlanetPopupViewUpgradeEnabled();
        }

        private void UpdatePlanetPopupViewUpgradeEnabled()
        {
            if (_selectedPlanet != null)
            {
                IsUpgradeEnabled = _selectedPlanet.CanUpgrade;
                StateChanged?.Invoke();
            }
        }

        private void BindPlanetPopup(IPlanet planet)
        {
            _selectedPlanet = planet;

            TitleText = _selectedPlanet.Name;
            Icon = _selectedPlanet.GetIcon(true);

            _selectedPlanet.OnPopulationChanged += HandleOnPopulationChanged;
            _selectedPlanet.OnUpgraded += HandleOnUpgraded;
            _selectedPlanet.OnIncomeChanged += HandleOnIncomeChanged;

            HandleOnPopulationChanged(_selectedPlanet.Population);
            HandleOnUpgraded(_selectedPlanet.Level);
            HandleOnIncomeChanged(_selectedPlanet.MinuteIncome);
            UpdatePlanetPopupViewUpgradeEnabled();

            _planetPopupView.BindPresenter(this);
            StateChanged?.Invoke();
        }

        private void UnbindPlanetPopup()
        {
            if(_selectedPlanet == null)
                return;

            _selectedPlanet.OnPopulationChanged -= HandleOnPopulationChanged;
            _selectedPlanet.OnUpgraded -= HandleOnPopulationChanged;
            _selectedPlanet.OnIncomeChanged -= HandleOnIncomeChanged;
            _planetPopupView.UnBindPresenter();
            _selectedPlanet = null;
        }

        private void HandleOnUpgraded(int level)
        {
            LevelText = ($"{level}/{_selectedPlanet.MaxLevel}");
            IsUpgradeEnabled = (_selectedPlanet.CanUpgrade);

            if (_selectedPlanet.Level != _selectedPlanet.MaxLevel)
            {
                UpgradeCostText = ($"{_selectedPlanet.Price}");
                IsMaxLevel = false;
            }
            else
            {
                UpgradeCostText = "Max Level";
                IsMaxLevel = true;
            }
            StateChanged?.Invoke();

        }

        private void HandleOnIncomeChanged(int income)
        {
            IncomeText = $"{income.ToString()}";
            StateChanged?.Invoke();
        }

        private void HandleOnPopulationChanged(int population)
        {
            PopulationText = population.ToString();
            StateChanged?.Invoke();
        }

        public void Close()
        {
            _planetPopupView.gameObject.SetActive(false);
            UnbindPlanetPopup();
        }

        public void Upgrade()
        {
            _selectedPlanet?.Upgrade();
        }

        public void Dispose()
        {
            _moneyStorage.OnMoneyChanged -= HandleOnMoneyChanged;

        }
    }
}