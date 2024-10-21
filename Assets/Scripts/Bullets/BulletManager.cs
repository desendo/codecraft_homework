using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField] private LevelBounds _levelBounds;
        [SerializeField] private BulletSpawnerAbstract _spawner;

        private void Awake()
        {
            _spawner.Prewarm(7);
        }

        private void OnEnable()
        {
            _spawner.OnSpawned += HandleOnSpawned;
            _spawner.OnDespawned += HandleOnDespawned;
        }

        private void HandleOnSpawned(Bullet bullet)
        {
            bullet.OnDestroyRequest += HandleBulletDestroy;
        }

        private void HandleOnDespawned(Bullet bullet)
        {
            bullet.OnDestroyRequest -= HandleBulletDestroy;
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

        private void HandleBulletDestroy(Bullet bullet)
        {
            RemoveBullet(bullet);
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