using UnityEngine;
using Zenject;

namespace Game.Game.Scripts.Pools
{
    public class CoinPool : MonoMemoryPool<Vector3, Vector3, CoinView>
    {
        protected override void Reinitialize(Vector3 start, Vector3 finish, CoinView item)
        {
            item.StartMoving(start, finish);
        }
    }
}