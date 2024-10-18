using System;
using UnityEngine;

namespace ShootEmUp
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private int _maxHealth;
        public event Action OnDeath;
        public bool IsDead => CurrentHealth <= 0;
        public int CurrentHealth { get; private set; }

        private void Awake()
        {
            DoReset();
        }

        private void OnDisable()
        {
            DoReset();
        }


        public void Damage(int damage)
        {
            if(CurrentHealth <= 0)
                return;

            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }

        private void DoReset()
        {
            CurrentHealth = _maxHealth;
        }
    }
}