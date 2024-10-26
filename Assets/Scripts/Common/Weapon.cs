using UnityEngine;

namespace ShootEmUp
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private bool _isPlayer;
        [SerializeField] private int _damage;
        [SerializeField] private Color _color;
        [SerializeField] private PhysicsLayer _physicsLayer;
        [SerializeField] private BulletManager _bulletSpawner;

        public void Init(BulletManager bulletSpawner)
        {
            _bulletSpawner = bulletSpawner;
        }

        public void Fire(Vector2 position, Vector2 direction)
        {
            _bulletSpawner.AddBullet(position, _color, (int) _physicsLayer, _damage, _isPlayer, direction * _bulletSpeed);
        }
    }
}