using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShootEmUp
{
    public class Spawner<T> where T : MonoBehaviour
    {
        private readonly T _prefab;
        private readonly Queue<T> _pool = new();
        private readonly List<T> _active = new();
        private readonly Transform _poolContainer;
        private readonly Transform _instancesParent;

        public event Action<T> OnSpawned;
        public event Action<T> OnDespawned;
        public IReadOnlyList<T> ActiveItems => _active;

        public Spawner(T prefab, Transform container, Transform worldTransform = null)
        {
            _prefab = prefab;
            _poolContainer = container;
            _poolContainer.gameObject.SetActive(false);
            _instancesParent = worldTransform;
        }

        public void Prewarm(int count = 5)
        {
            for (int i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(_prefab, _poolContainer);
                instance.enabled = false;
                _pool.Enqueue(instance);
            }
        }

        public T Spawn()
        {
            if (!_pool.TryDequeue(out var instance))
            {
                instance = Object.Instantiate(_prefab, _poolContainer);
            }

            instance.transform.SetParent(_instancesParent);
            _active.Add(instance);
            OnSpawned?.Invoke(instance);
            return instance;
        }

        public void Despawn(T instance)
        {
            instance.transform.SetParent(_poolContainer);
            _active.Remove(instance);
            _pool.Enqueue(instance);
            OnDespawned?.Invoke(instance);
        }
    }
}