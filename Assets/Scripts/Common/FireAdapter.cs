using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShootEmUp
{
    public class FireAdapter : MonoBehaviour
    {
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private bool _isPlayer;
        [SerializeField] private int _damage;
        [SerializeField] private Color _color;
        [SerializeField] private PhysicsLayer _physicsLayer;
        [SerializeField] private BulletManager _bulletManager;
        [SerializeField] private GameObject _fireRequester;

        private void Awake()
        {
            if (_fireRequester != null && _fireRequester.TryGetComponent<IFireRequester>(out var requester))
                AddSubscriber(requester);
        }

        private void Fire(Vector2 position, Vector2 direction)
        {
            _bulletManager.AddBullet(position, _color, (int) _physicsLayer, _damage, _isPlayer, direction * _bulletSpeed);
        }

        public void AddSubscriber(IFireRequester fireRequester)
        {
            fireRequester.FireRequested += Fire;
        }

        public void RemoveSubscriber(IFireRequester fireRequester)
        {
            fireRequester.FireRequested -= Fire;
        }
    }
}