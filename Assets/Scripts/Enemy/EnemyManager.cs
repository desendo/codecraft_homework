using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootEmUp
{
    public sealed class EnemyManager : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;
        [SerializeField] private Player _character;

        [SerializeField] private FireAdapter _fireAdapter;
        [SerializeField] private Enemy prefab;

        [SerializeField] public Transform _worldTransform;
        [SerializeField] private Transform _inactiveContainer;

        private Spawner<Enemy> _spawner;
        private void Awake()
        {
            _spawner = new Spawner<Enemy>(prefab, _inactiveContainer, _worldTransform);
            _spawner.Prewarm(7);
            _spawner.OnSpawned += HandleOnEnemyAdded;
            _spawner.OnDespawned += HandleOnEnemyRemoved;
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));

                if (_spawner.ActiveItems.Count >= 5)
                    continue;

                AddEnemy();
            }
        }

        private void AddEnemy()
        {
            var enemy = _spawner.Spawn();
            enemy.transform.position = GetRandomPoint(_spawnPositions);
            enemy.SetDestination(GetRandomPoint(_attackPositions));
            enemy.Target = _character;
        }

        private void HandleOnEnemyAdded(Enemy enemy)
        {
            enemy.FireRequest += _fireAdapter.Fire;
            enemy.OnDeath += HandleEnemyDeath;
        }

        private void HandleOnEnemyRemoved(Enemy enemy)
        {
            enemy.FireRequest -= _fireAdapter.Fire;
            enemy.OnDeath -= HandleEnemyDeath;
        }

        private void HandleEnemyDeath(Enemy enemy)
        {
            _spawner.Despawn(enemy);
        }



        private Vector3 GetRandomPoint(Transform[] points)
        {
            var index = Random.Range(0, points.Length);
            return points[index].position;
        }
    }
}