using DG.Tweening;
using Game.Game.Scripts;
using TMPro;
using UnityEngine;

namespace Game
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyCounter;
        [SerializeField] private RectTransform _anchor;

        private int _money;
        private Sequence _sequence;
        private int _targetValue;
        public RectTransform FlyAnchor => _anchor;

        private int Money
        {
            get => _money;
            set
            {
                _money = value;
                _moneyCounter.text = value.ToString();
            }
        }

        public enum AnimationType
        {
            None,
            Automatic,
            Manual
        }
        public void SetMoney(int targetValue, AnimationType animationType)
        {
            switch (animationType)
            {
                case AnimationType.Manual:
                    _targetValue = targetValue;
                    break;
                case AnimationType.Automatic:
                    _targetValue = targetValue;
                    Animate();
                    break;
                default:
                    Money = targetValue;
                    break;
            }
        }
        public void Animate()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence()
                    .Append(DOTween.To(() => Money, x => Money = x, _targetValue, Const.CoinCounterTime));
        }
    }
}
