using UnityEngine;
namespace Game.Scripts.Components
{
    public class GroundedComponent : MonoBehaviour
    {
        [SerializeField] private LayerMask _layerMaskGround;
        [SerializeField] private Transform _feet;
        [SerializeField] private bool _isGrounded;
        [SerializeField] private float _detectionDistance = 0.1f;
        private Vector2 _groundVelocity;

        public Vector2 GroundVelocity => _groundVelocity;
        public bool IsGrounded => _isGrounded;
        private void FixedUpdate()
        {
            _groundVelocity = Vector2.zero;
            _isGrounded = false;
            var hit = Physics2D.Raycast(_feet.position, Vector2.down, _detectionDistance, _layerMaskGround);
            if (hit)
            {
                _isGrounded = true;
                if(hit.collider.attachedRigidbody != null)
                    _groundVelocity = hit.collider.attachedRigidbody.velocity;
            }
        }
    }
}

