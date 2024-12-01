using Modules.Money;
using Modules.Planets;

namespace Game.Game.Scripts.UI.Presenters
{
    public class PlanetPopupPresenter
    {
        private readonly PlanetPopupView _planetPopupView;
        private IPlanet _selectedPlanet;
        private readonly IMoneyStorage _moneyStorage;

        public PlanetPopupPresenter(PlanetPopupView planetPopupView, IMoneyStorage moneyStorage)
        {
            _moneyStorage = moneyStorage;
            _planetPopupView = planetPopupView;
            _planetPopupView.OnClose += HandleCloseRequest;
            _planetPopupView.OnUpgrade += HandleUpgradeRequest;
            _moneyStorage.OnMoneyChanged += (x,y)=>HandleOnMoneyChanged();

            HandleOnMoneyChanged();
        }

        public void OpenPopup(IPlanet planet)
        {
            if(!planet.IsUnlocked)
                return;

            _planetPopupView.gameObject.SetActive(true);
            BindPlanetPopup(planet);
        }

        private void HandleOnMoneyChanged()
        {
            if(_selectedPlanet != null)
                _planetPopupView.SetUpgradeEnabled(_selectedPlanet.CanUpgrade);
        }



        private void BindPlanetPopup(IPlanet planet)
        {
            _selectedPlanet = planet;

            _selectedPlanet.OnPopulationChanged += HandleOnPopulationChanged;
            _selectedPlanet.OnUpgraded += HandleOnUpgraded;
            _selectedPlanet.OnIncomeChanged += HandleOnIncomeChanged;

            HandleOnUpgraded(_selectedPlanet.Level);
            HandleOnIncomeChanged(_selectedPlanet.MinuteIncome);
            HandleOnMoneyChanged();
            _planetPopupView.SetTitle(_selectedPlanet.Name);
            _planetPopupView.SetIcon(_selectedPlanet.GetIcon(true));

        }

        private void HandleOnUpgraded(int level)
        {
            _planetPopupView.SetLevel($"{level}/{_selectedPlanet.MaxLevel}");
            _planetPopupView.SetUpgradeCost($"{_selectedPlanet.Price}");
        }

        private void HandleOnIncomeChanged(int income)
        {
            _planetPopupView.SetIncome($"{income.ToString()}");
        }

        private void HandleUpgradeRequest()
        {
            _selectedPlanet?.Upgrade();
        }
        private void HandleCloseRequest()
        {
            _planetPopupView.gameObject.SetActive(false);
            UnbindPlanetPopup(_selectedPlanet);
        }
        private void HandleOnPopulationChanged(int population)
        {
            _planetPopupView.SetPopulation(population.ToString());
        }

        private void UnbindPlanetPopup(IPlanet planet)
        {
            if(_selectedPlanet == null)
                return;

            _selectedPlanet.OnPopulationChanged += HandleOnPopulationChanged;
            _selectedPlanet.OnUpgraded += HandleOnPopulationChanged;
            _selectedPlanet.OnIncomeChanged += HandleOnIncomeChanged;

            _selectedPlanet = null;
        }
    }
}