using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class LevelBackground : MonoBehaviour
    {
        private float _startPositionY;
        private float _endPositionY;
        private float _movingSpeedY;
        private float _positionX;
        private float _positionZ;

        private Transform _selfTransform;

        [SerializeField] private Params _params;

        private void Awake()
        {
            _startPositionY = _params._startPositionY;
            _endPositionY = _params._endPositionY;
            _movingSpeedY = _params._movingSpeedY;
            _selfTransform = transform;
            var position = _selfTransform.position;
            _positionX = position.x;
            _positionZ = position.z;
        }

        private void FixedUpdate()
        {
            if (_selfTransform.position.y <= _endPositionY)
            {
                _selfTransform.position = new Vector3(
                    _positionX,
                    _startPositionY,
                    _positionZ
                );
            }

            _selfTransform.position -= new Vector3(
                _positionX,
                _movingSpeedY * Time.fixedDeltaTime,
                _positionZ
            );
        }

        [Serializable]
        public sealed class Params
        {
            [SerializeField]
            public float _startPositionY;

            [SerializeField]
            public float _endPositionY;

            [SerializeField]
            public float _movingSpeedY;
        }
    }
}