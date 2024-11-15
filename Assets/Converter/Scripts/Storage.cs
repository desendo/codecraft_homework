using System;
using UnityEngine;

namespace ConverterModule.Scripts
{
    public class Storage
    {
        private readonly int _capacity;
        private int _current;

        public event Action<int> OnChange;
        public int Current => _current;
        /// <summary>
        /// хранилище ограниченной вместимости с методами взять да положить
        /// </summary>
        /// <param name="capacity">вместимость</param>
        /// <exception cref="ArgumentOutOfRangeException"> капасити должно быть больше нуля</exception>
        public Storage(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException();

            _capacity = capacity;
        }

        public bool Take(int count)
        {
            if (_current >= count)
            {
                _current -= count;
                OnChange?.Invoke(Current);
                return true;
            }

            return false;
        }

        public void Put(int count, out int change)
        {
            if (count <= 0)
                throw new ArgumentOutOfRangeException();

            var target = _current + count;
            change = Mathf.Max(0, target - _capacity);
            if (target > _capacity)
                target = _capacity;

            _current = target;
            OnChange?.Invoke(Current);
        }
    }
}