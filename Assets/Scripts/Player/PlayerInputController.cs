using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private Player _character;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _character.Fire();

            var moveDirection = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
                moveDirection.x += -1;
            if (Input.GetKey(KeyCode.RightArrow))
                moveDirection.x += 1;

            _character.SetMoveDirection(moveDirection);
        }
    }
}