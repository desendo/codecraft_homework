using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Scripts.Components
{
    public class WaypointMoverComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] _waypoints;
        [SerializeField] private Transform _positionHandler;
        [SerializeField] private MoveComponent _moveComponent;

        private int _targetIndex;
        private readonly float _toleranceSqr = 0.1f;

        private bool _isAtPoint = true;
        private void Update()
        {

            if (_isAtPoint)
            {
                _targetIndex++;
                if (_targetIndex >= _waypoints.Length)
                    _targetIndex = 0;

                _isAtPoint = false;
                var delta = _waypoints[_targetIndex].position - _positionHandler.position;
                _moveComponent.SetMoveDirection(delta.normalized);
            }
            else
            {
                var delta = _waypoints[_targetIndex].position - _positionHandler.position;
                if (delta.sqrMagnitude < _toleranceSqr)
                    _isAtPoint = true;
            }
        }
    }
}
