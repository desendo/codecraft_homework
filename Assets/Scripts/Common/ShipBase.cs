using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public class Ship : MonoBehaviour, IUnit
    {
        [SerializeField] protected Health _health;
        [SerializeField] protected bool _isPlayer;
        [SerializeField] protected Transform _firePoint;
        [SerializeField] private Weapon _weapon;

        public bool IsAlive => !_health.IsDead;
        public Transform FirePoint => _firePoint;
        public bool IsPlayer => _isPlayer;
        public void Damage(int damage) => _health.Damage(damage);
        public Action<Ship> OnDeath;

        private void Awake()
        {
            _health.OnDeath += ()=> OnDeath.Invoke(this);
        }

        public void Fire()
        {
            _weapon.Fire(_firePoint.position,_firePoint.rotation * Vector2.up );
        }

        public void Fire(Vector2 dir)
        {
            _weapon.Fire(_firePoint.position, dir);
        }
    }

}