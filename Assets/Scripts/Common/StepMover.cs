using UnityEngine;

namespace ShootEmUp
{
    public class StepMover : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] public float _speed = 1;
        public Vector2 Position => transform.position;

        public void MoveStep(Vector2 step)
        {
            var targetPosition = _rigidbody2D.position + step * _speed;
            _rigidbody2D.MovePosition(targetPosition);
        }
    }
}