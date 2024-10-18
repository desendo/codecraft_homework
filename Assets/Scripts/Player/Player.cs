using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : MonoBehaviour, IUnit
    {

        [SerializeField] private Health _health;
        [SerializeField] private StepMover _stepMover;
        [SerializeField] private bool _isPlayer;
        [SerializeField] public Transform firePoint;

        public Action<Player> OnDeath;
        private Vector2 _moveDirection;
        public delegate void FireHandler(Vector2 position, Vector2 direction);
        public event FireHandler FireRequested;

        public bool IsAlive => !_health.IsDead;
        public bool IsPlayer => _isPlayer;

        public void Damage(int damage) => _health.Damage(damage);
        private void OnEnable() => _health.OnDeath += HandleHealthOnDeath;
        private void OnDisable() => _health.OnDeath -= HandleHealthOnDeath;
        private void HandleHealthOnDeath() => OnDeath.Invoke(this);

        public void FixedUpdate()
        {
            _stepMover.MoveStep(_moveDirection * Time.fixedDeltaTime);
        }

        public void Fire()
        {
            FireRequested?.Invoke(firePoint.position,firePoint.rotation * Vector2.up );
        }

        public void Move(Vector2 moveDirectionNormalized)
        {
            _moveDirection = moveDirectionNormalized;
        }
    }
}