using UnityEngine;

namespace ShootEmUp
{
    public sealed class PlayerController : MonoBehaviour
    {
        [SerializeField] private Player _character;
        [SerializeField] private FireAdapter _fireAdapter;

        private void Awake()
        {
            _character.OnDeath += _ => Time.timeScale = 0;
            _character.FireRequested += HandlePlayerFireRequested;
        }

        private void HandlePlayerFireRequested(Vector2 position, Vector2 direction)
        {
            _fireAdapter.Fire(position, direction);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                _character.Fire();

            var moveDirection = Vector2.zero;

            if (Input.GetKey(KeyCode.LeftArrow))
                moveDirection.x += -1;
            if (Input.GetKey(KeyCode.RightArrow))
                moveDirection.x += 1;

            _character.Move(moveDirection);
        }

    }
}