using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootEmUp
{
    public class EnemyShipService : MonoBehaviour
    {
        [SerializeField] private Ship _target;
        [SerializeField] private BulletManager _bulletManager;

        [SerializeField] private EnemySpawner _spawner;
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;
        public IReadOnlyList<Ship> ActiveItems => _spawner.ActiveItems;

        public Ship AddShip()
        {
            var enemy = _spawner.Spawn();
            enemy.transform.position = GetRandomPoint(_spawnPositions);

            var weapon = enemy.GetComponent<Weapon>();
            weapon.Init(_bulletManager);

            var ai = enemy.GetComponent<EnemyAI>();
            ai.SetDestination(GetRandomPoint(_attackPositions));
            ai.Target = _target;
            return enemy;
        }
        private Vector3 GetRandomPoint(Transform[] points)
        {
            var index = Random.Range(0, points.Length);
            return points[index].position;
        }

        public void RemoveShip(Ship enemy)
        {
            _spawner.Despawn(enemy);
        }
    }
}