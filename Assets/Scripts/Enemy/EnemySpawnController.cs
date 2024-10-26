using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ShootEmUp
{

    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyShipService _enemyShipService;

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(1, 2));

                if (_enemyShipService.ActiveItems.Count >= 5)
                    continue;

                var ship = _enemyShipService.AddShip();
                ship.OnDeath += HandleEnemyDeath;

            }
        }

        private void HandleEnemyDeath(Ship enemy)
        {
            _enemyShipService.RemoveShip(enemy);
            enemy.OnDeath -= HandleEnemyDeath;
        }
    }
}