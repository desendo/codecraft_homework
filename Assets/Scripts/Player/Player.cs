using UnityEngine;

namespace ShootEmUp
{
    public sealed class Player : ShipBase
    {
        [SerializeField] private StepMover _stepMover;
        private Vector2 _moveDirection;

        public void FixedUpdate()
        {
            _stepMover.MoveStep(_moveDirection * Time.fixedDeltaTime);
        }
        public void SetMoveDirection(Vector2 moveDirectionNormalized)
        {
            _moveDirection = moveDirectionNormalized;
        }
    }
}