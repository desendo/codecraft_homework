using Modules;
using UnityEngine;
using Zenject;

namespace Game.Pools
{
    public class CoinsPool: MonoMemoryPool<Vector2Int, Coin>, ICoinsSpawner
    {

        protected override void Reinitialize(Vector2Int pos, Coin item)
        {
            base.Reinitialize(pos, item);
            item.Generate();
            item.Position = pos;
        }

    }

    public interface ICoinsSpawner : IMemoryPool<Vector2Int, Coin>
    {
    }
}