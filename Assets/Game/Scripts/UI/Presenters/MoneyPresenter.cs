using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
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
            moneyView.Money = moneyStorage.Money;
            moneyStorage.OnMoneyEarned += HandleOnMoneyEarned;
            moneyStorage.OnMoneySpent += HandleOnMoneySpent;
        }

        private void HandleOnMoneySpent(int newvalue, int range)
        {
            _sequence?.Kill();
            _moneyView.Money = newvalue;
        }

        private void HandleOnMoneyEarned(int newvalue, int range)
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(Const.CoinFlyTime);
            _sequence.Append(DOTween.To(() => _moneyView.Money, x => _moneyView.Money = x, newvalue, Const.CoinCounterTime));
        }
    }
}