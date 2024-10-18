using System;
using UnityEngine;

namespace ShootEmUp
{
    public class DestinationMover : MonoBehaviour
    {

        [SerializeField] private StepMover _stepMover;
        private Vector2 _direction;
        private bool _targetSet;
        private Vector2 _target;
        private readonly float _distanceTolerance = 0.25f;
        public event Action OnDestination;
        public void SetTarget(Vector2 target)
        {
            _targetSet = true;
            _target = target;
            _direction = (target - _stepMover.Position).normalized;
        }

        private void FixedUpdate()
        {
            if(!_targetSet)
                return;

            _stepMover.MoveStep(_direction * Time.fixedDeltaTime);

            var magnitude = (_stepMover.Position - _target).magnitude;
            if (magnitude < _distanceTolerance)
            {
                OnDestination?.Invoke();
                _targetSet = false;
            }
        }
    }
}