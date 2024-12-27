using UnityEngine;
using UnityEngine.Events;
namespace Game.Scripts.Components
{
    public class MoveComponent : MonoBehaviour
    {

        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private MoveType _moveType;
        [SerializeField] private Vector2 _addForceSpeed;

        [SerializeField] private UnityEvent<bool> _onDirectionRight;
        private Vector2 _dir;

        public enum MoveType
        {
            Simple,
            AddForce
        }

        private void FixedUpdate()
        {
            var targetVelocity = _dir * _speed;
            if (_moveType == MoveType.Simple)
            {
                transform.position += (Vector3)targetVelocity * Time.fixedDeltaTime;
            }
            else if (_moveType == MoveType.AddForce)
            {
                var addForce = Vector2.zero;
                var targetVelocityNormalized = targetVelocity.normalized;
                if (targetVelocity.x < 0 && _rigidbody2D.velocity.x > targetVelocity.x)
                    addForce.x = targetVelocityNormalized.x;
                else if (targetVelocity.x > 0 && _rigidbody2D.velocity.x < targetVelocity.x)
                    addForce.x = targetVelocityNormalized.x;
                if (targetVelocity.y < 0 && _rigidbody2D.velocity.y > targetVelocity.y)
                    addForce.y = targetVelocityNormalized.y;
                else if (targetVelocity.y > 0 && _rigidbody2D.velocity.y < targetVelocity.y)
                    addForce.y = targetVelocityNormalized.y;
                _rigidbody2D.AddForce(addForce * Time.fixedDeltaTime * _addForceSpeed, ForceMode2D.Impulse);
            }
        }

        public void SetMoveDirection(Vector2 dir)
        {
            _dir = Vector2.ClampMagnitude(dir, 1);

            if(_dir.x > 0)
                _onDirectionRight.Invoke(true);
            if(_dir.x < 0)
                _onDirectionRight.Invoke(false);
        }
    }
}