using DG.Tweening;
using Modules.Money;

namespace Game.Game.Scripts.UI.Presenters
{
    public class MoneyPresenter
    {
        private readonly MoneyView _moneyView;
        private Sequence _sequence;

        public MoneyPresenter(MoneyView moneyView, IMoneyStorage moneyStorage)
        {
            _moneyView = moneyView;
            _moneyView.SetMoney(moneyStorage.Money, MoneyView.AnimationType.None);
            moneyStorage.OnMoneyEarned += HandleOnMoneyEarned;
            moneyStorage.OnMoneySpent += HandleOnMoneySpent;
        }

        private void HandleOnMoneySpent(int newValue, int range)
        {
            _moneyView.SetMoney(newValue, MoneyView.AnimationType.Automatic);
        }

        private void HandleOnMoneyEarned(int newValue, int range)
        {
            _moneyView.SetMoney(newValue, MoneyView.AnimationType.Manual);
        }
    }
}