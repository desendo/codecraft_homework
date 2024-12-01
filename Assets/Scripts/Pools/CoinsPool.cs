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

        protected override void OnCreated(Coin item)
        {
            base.OnCreated(item);
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(Coin item)
        {
            base.OnSpawned(item);
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Coin item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }
    }

    public interface ICoinsSpawner : IMemoryPool<Vector2Int, Coin>
    {
    }
}