using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts.Components
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _max;
        [SerializeField] private int _current;

        [SerializeField] private UnityEvent _onDead;
        [SerializeField] private UnityEvent _onGetDamaged;
        public bool IsDead => _current <= 0;

        public void Damage(int damage)
        {
            if(IsDead)
                return;

            if(damage <= 0)
                return;

            _current-=damage;
            _onGetDamaged?.Invoke();

            if(IsDead)
                _onDead?.Invoke();
        }
    }
}
