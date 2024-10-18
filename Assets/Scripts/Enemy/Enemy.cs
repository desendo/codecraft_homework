using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Enemy : MonoBehaviour, IUnit
    {
        public delegate void FireHandler(Vector2 position, Vector2 direction);

        [SerializeField] public bool _isPlayer;
        [SerializeField] public Transform firePoint;
        [SerializeField] private float _countdown;
        [SerializeField] private DestinationMover _destinationMover;
        [SerializeField] private Health _health;
        [SerializeField] private Timer _shootTimer;
        private float _currentTime;
        private Vector2 _destination;
        public Player Target { get; set; }
        public bool IsPlayer => _isPlayer;
        public event Action<Enemy> OnDeath;
        public event FireHandler FireRequest;

        private void Awake()
        {
            _health.OnDeath += () => OnDeath?.Invoke(this);
            _destinationMover.OnDestination += () => _shootTimer.StartTimer();

            _shootTimer.Setup(_countdown, true);
            _shootTimer.OnTime += TryShoot;
        }

        private void TryShoot()
        {
            Vector2 startPosition = firePoint.position;
            var delta = (Vector2)Target.transform.position - startPosition;
            var direction = delta.normalized;
            FireRequest?.Invoke(startPosition, direction);
            _shootTimer.RestartTimer();
        }

        public void Damage(int damage)
        {
            _health.Damage(damage);
        }

        public void SetDestination(Vector2 endPoint)
        {
           _destinationMover.SetTarget(endPoint);
        }
    }
}