using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private Ship _playerShip;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _playerShip.Fire();

            var moveDirection = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
                moveDirection.x += -1;
            if (Input.GetKey(KeyCode.RightArrow))
                moveDirection.x += 1;

            _playerMover.SetMoveDirection(moveDirection);
        }
    }
}