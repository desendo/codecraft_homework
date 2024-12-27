using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts.Components
{
    public class InputComponent : MonoBehaviour
    {
        [SerializeField] private MoveComponent _moveComponent;
        [SerializeField] private JumpComponent _jumpComponent;
        [SerializeField] private AddForceComponent _tossComponent;
        [SerializeField] private AddForceComponent _pushComponent;
        private void Update()
        {
            _moveComponent.SetMoveDirection(new Vector2(Input.GetAxisRaw("Horizontal"),0));
            if(Input.GetKeyDown(KeyCode.Space))
                _jumpComponent.TryJump();
            if(Input.GetMouseButtonDown(1))
                _tossComponent.TryForce();
            if(Input.GetMouseButtonDown(0))
                _pushComponent.TryForce();
        }
    }
}