using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField] public Bullet _prefab;
        [SerializeField] public Transform _worldTransform;
        [SerializeField] private LevelBounds _levelBounds;
        [SerializeField] private Transform _inactiveContainer;

        private Spawner<Bullet> _spawner;

        private void Awake()
        {
            _spawner = new Spawner<Bullet>(_prefab, _inactiveContainer, _worldTransform);
            _spawner.Prewarm(7);
        }

        private void OnEnable()
        {
            _spawner.OnSpawned += HandleOnSpawned;
            _spawner.OnDespawned += HandleOnDespawned;
        }

        private void HandleOnSpawned(Bullet bullet)
        {
            bullet.OnCollisionEntered += OnBulletCollision;
        }

        private void HandleOnDespawned(Bullet bullet)
        {
            bullet.OnCollisionEntered -= OnBulletCollision;
        }

        private void FixedUpdate()
        {
            CheckIsInLevelBounds();
        }

        private void CheckIsInLevelBounds()
        {
            foreach (var item in _spawner.ActiveItems)
            {
                if (_levelBounds.InBounds(item.transform.position)) continue;

                RemoveBullet(item);
                return;
            }
        }

        public void AddBullet(Vector2 position, Color color, int physicsLayer, int damage,
            bool isPlayer, Vector2 velocity)
        {
            var bullet = _spawner.Spawn();
            bullet.Setup(position, color, physicsLayer, damage, isPlayer, velocity);
        }

        private void OnBulletCollision(Bullet bullet, Collision2D collision)
        {
            TryDealDamage(bullet, collision.gameObject);
            RemoveBullet(bullet);
        }

        private bool TryDealDamage(Bullet bullet, GameObject other)
        {
            int damage = bullet.Damage;
            if (damage <= 0)
                return false;

            if (other.TryGetComponent<IUnit>(out var unit))
            {
                if (bullet.IsPlayer == unit.IsPlayer)
                    return false;

                unit.Damage(damage);
                return true;
            }
            return false;
        }

        private void RemoveBullet(Bullet bullet)
        {
            _spawner.Despawn(bullet);
        }

        private void OnDisable()
        {
            _spawner.OnSpawned -= HandleOnSpawned;
            _spawner.OnDespawned -= HandleOnDespawned;
        }
    }
}