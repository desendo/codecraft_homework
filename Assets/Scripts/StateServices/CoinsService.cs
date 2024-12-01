using System;
using System.Collections.Generic;
using Game.Pools;
using Modules;
using SnakeGame;
using UnityEngine;
using Zenject;

namespace Game.StateServices
{
    public interface ICoinsService
    {
        event Action OnCoinsChanged;
        int LevelCoinsLeft { get; }
        int PickedCoins { get; }
        void GenerateCoins(int count);
        bool TryPickCoin(Vector2Int pos, out int coinScore, out int coinBones);
    }

    public class CoinsService : ICoinsService
    {
        private readonly ICoinsSpawner _coinPool;
        private readonly IWorldBounds _worldBounds;

        public event Action OnCoinsChanged;
        public int LevelCoinsLeft => _coinsByPositions.Count;
        public int PickedCoins => _pickedCoins;

        private readonly Dictionary<Vector2Int, Coin> _coinsByPositions = new Dictionary<Vector2Int, Coin>();
        private int _pickedCoins;

        public CoinsService(ICoinsSpawner coinPool, IWorldBounds worldBounds)
        {
            _coinPool = coinPool;
            _worldBounds = worldBounds;
        }
        public void GenerateCoins(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var coin = _coinPool.Spawn(_worldBounds.GetRandomPosition());
                _coinsByPositions.Add(coin.Position, coin);
            }

            _pickedCoins = 0;
            OnCoinsChanged?.Invoke();
        }


        public bool TryPickCoin(Vector2Int pos, out int coinScore, out int coinBones)
        {
            if (_coinsByPositions.TryGetValue(pos, out var coin))
            {
                _coinPool.Despawn(coin);
                _coinsByPositions.Remove(pos);

                _pickedCoins++;
                OnCoinsChanged?.Invoke();
                coinBones = coin.Bones;
                coinScore = coin.Score;
                return true;
            }

            coinScore = 0;
            coinBones = 0;
            return false;
        }
    }
}