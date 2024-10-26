using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float _countdown;
        [SerializeField] private DestinationMover _destinationMover;
        [SerializeField] private Timer _shootTimer;
        [SerializeField] private Ship _ship;
        private float _currentTime;
        private Vector2 _destination;
        public Ship Target { get; set; }

        private void Awake()
        {
            _destinationMover.OnDestination += () => _shootTimer.StartTimer();
            _shootTimer.OnTime += Shoot;
            _shootTimer.Setup(_countdown, true);
        }

        private void Shoot()
        {
            Vector2 startPosition = _ship.FirePoint.position;
            var delta = (Vector2)Target.transform.position - startPosition;
            _ship.Fire(delta.normalized);
            _shootTimer.RestartTimer();
        }

        public void SetDestination(Vector2 endPoint)
        {
            _destinationMover.SetTarget(endPoint);
        }
    }
}