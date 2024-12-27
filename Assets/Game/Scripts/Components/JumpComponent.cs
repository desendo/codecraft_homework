using System.Collections.Generic;
using Game.Scripts.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts.Components
{
    public class JumpComponent : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private GroundedComponent _groundedComponent;
        [SerializeField] private CooldownComponent _cooldownComponent;
        [SerializeField] private UnityEvent _onJump;
        [SerializeField] private float _jumpForce;

        public void TryJump()
        {
            if (_groundedComponent.IsGrounded)
            {
                if (_cooldownComponent.IsReady)
                {
                    _rigidbody2D.AddForce(Vector2.up * _jumpForce);
                    _onJump?.Invoke();
                    _cooldownComponent.Cooldown();
                }
            }
        }
    }
}