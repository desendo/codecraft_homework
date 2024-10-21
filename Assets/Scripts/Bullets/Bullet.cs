using System;
using UnityEngine;

namespace ShootEmUp
{
    public sealed class Bullet : MonoBehaviour
    {

        [SerializeField] public new Rigidbody2D rigidbody2D;
        [SerializeField] public SpriteRenderer spriteRenderer;

        public event Action<Bullet, Collision2D> OnCollisionEntered;

        public bool IsPlayer { get; private set; }

        public int Damage { get; private set; }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            TryDealDamage(collision.gameObject);
            OnCollisionEntered?.Invoke(this, collision);
        }

        private void TryDealDamage(GameObject other)
        {
            if (Damage <= 0) return;

            if (!other.TryGetComponent<IUnit>(out var unit)) return;

            if (IsPlayer != unit.IsPlayer)
                unit.Damage(Damage);
        }

        public void Setup(Vector2 position, Color color, int physicsLayer, int damage, bool isPlayer, Vector2 velocity)
        {
            transform.position = position;
            spriteRenderer.color = color;
            gameObject.layer = physicsLayer;
            Damage = damage;
            IsPlayer = isPlayer;
            rigidbody2D.velocity = velocity;
        }
    }
}