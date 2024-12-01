using TMPro;
using UnityEngine;

namespace Game
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _moneyCounter;
        [SerializeField] private RectTransform _anchor;

        private int _money;
        public RectTransform FlyAnchor => _anchor;
        public int Money
        {
            get => _money;
            set
            {
                _money = value;
                _moneyCounter.text = value.ToString();
            }
        }

    }
}
