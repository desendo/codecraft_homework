using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ShootEmUp
{

    public sealed class EnemySpawnController : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;
        [SerializeField] private Player _character;
        [SerializeField] private FireAdapter _fireAdapter;
        [SerializeField] private EnemySpawner _spawner;
        private void Awake()
        {
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

            var ai = enemy.GetComponent<EnemyAI>();
            ai.SetDestination(GetRandomPoint(_attackPositions));
            ai.Target = _character;

        }

        private void HandleOnEnemyAdded(Enemy enemy)
        {
            enemy.OnDeath += HandleEnemyDeath;
            _fireAdapter.AddSubscriber(enemy);
        }

        private void HandleOnEnemyRemoved(Enemy enemy)
        {
            enemy.OnDeath -= HandleEnemyDeath;
            _fireAdapter.RemoveSubscriber(enemy);
        }

        private void HandleEnemyDeath(ShipBase enemy)
        {
            _spawner.Despawn(enemy as Enemy);
        }

        private Vector3 GetRandomPoint(Transform[] points)
        {
            var index = Random.Range(0, points.Length);
            return points[index].position;
        }
    }
}