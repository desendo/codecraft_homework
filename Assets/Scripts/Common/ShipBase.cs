using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public interface IFireRequester
    {
        public event FireHandler FireRequested;
        public delegate void FireHandler(Vector2 position, Vector2 direction);
    }

    public class ShipBase : MonoBehaviour, IUnit, IFireRequester
    {
        [SerializeField] protected Health _health;
        [SerializeField] protected bool _isPlayer;
        [SerializeField] protected Transform _firePoint;
        public event IFireRequester.FireHandler FireRequested;

        public bool IsAlive => !_health.IsDead;
        public Transform FirePoint => _firePoint;
        public bool IsPlayer => _isPlayer;
        public void Damage(int damage) => _health.Damage(damage);
        public Action<ShipBase> OnDeath;

        private void Awake()
        {
            _health.OnDeath += ()=> OnDeath.Invoke(this);
        }

        public void Fire()
        {
            FireRequested?.Invoke(_firePoint.position,_firePoint.rotation * Vector2.up );
        }

        public void Fire(Vector2 dir)
        {
            FireRequested?.Invoke(_firePoint.position,dir);
        }
    }

}