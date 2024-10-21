using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShootEmUp
{

    public abstract class SpawnerAbstract<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private T _prefab;
        [SerializeField] public Transform _instancesParent;
        [SerializeField] private Transform _poolContainer;
        private readonly Queue<T> _pool = new();
        private readonly List<T> _active = new();

        public event Action<T> OnDespawned;
        public event Action<T> OnSpawned;
        public IReadOnlyList<T> ActiveItems => _active;

        public void Awake()
        {
            _poolContainer.gameObject.SetActive(false);
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