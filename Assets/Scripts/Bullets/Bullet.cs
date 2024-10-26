using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {

        [SerializeField] public new Rigidbody2D rigidbody2D;
        [SerializeField] public SpriteRenderer spriteRenderer;

        private bool _isPlayer;
        private int _damage;

        public event Action<Bullet> OnDestroyRequest;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDealDamage(collision.gameObject);
            OnDestroyRequest?.Invoke(this);
        }

        private void TryDealDamage(GameObject other)
        {
            if (_damage <= 0) return;

            if (!other.TryGetComponent<IUnit>(out var unit)) return;

            if (_isPlayer != unit.IsPlayer)
                unit.Damage(_damage);
        }

        public void Setup(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            transform.position = position;
            spriteRenderer.color = color;
            gameObject.layer = physicsLayer;
            _damage = damage;
            _isPlayer = isPlayer;
            rigidbody2D.velocity = velocity;
        }
    }
}