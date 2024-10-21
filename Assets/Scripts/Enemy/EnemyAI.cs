using UnityEngine;

namespace ShootEmUp
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float _countdown;
        [SerializeField] private DestinationMover _destinationMover;
        [SerializeField] private Timer _shootTimer;
        [SerializeField] private ShipBase _shipBase;
        private float _currentTime;
        private Vector2 _destination;
        public Player Target { get; set; }

        private void Awake()
        {
            _destinationMover.OnDestination += () => _shootTimer.StartTimer();
            _shootTimer.Setup(_countdown, true);
            _shootTimer.OnTime += Shoot;
        }

        private void Shoot()
        {
            Vector2 startPosition = _shipBase.FirePoint.position;
            var delta = (Vector2)Target.transform.position - startPosition;
            _shipBase.Fire(delta.normalized);
            _shootTimer.RestartTimer();
        }

        public void SetDestination(Vector2 endPoint)
        {
            _destinationMover.SetTarget(endPoint);
        }
    }
}