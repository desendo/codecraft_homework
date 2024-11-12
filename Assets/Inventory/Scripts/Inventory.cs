using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ReSharper disable NotResolvedInText

namespace Inventories
{
    public sealed class Inventory : IEnumerable<Item>
    {
        private readonly int _width;
        private readonly int _height;
        private readonly Dictionary<Vector2Int, Item> _cellsOccupiedItems = new Dictionary<Vector2Int, Item>();
        private readonly Dictionary<Item, Vector2Int> _itemPositions = new Dictionary<Item, Vector2Int>();
        private readonly Dictionary<int, int> _countByNames = new Dictionary<int, int>();
        private readonly List<Item> _itemsList = new List<Item>();
        public event Action<Item, Vector2Int> OnAdded;
        public event Action<Item, Vector2Int> OnRemoved;
        public event Action<Item, Vector2Int> OnMoved;
        public event Action OnCleared;

        public int Width => _width;
        public int Height => _height;
        public int Count => _itemsList.Count;

        public Inventory(in int width, in int height)
        {
            if (width <= 0 || height <= 0)
                throw new ArgumentOutOfRangeException();

            _width = width;
            _height = height;

            InitCells();
        }

        public Inventory(
            in int width,
            in int height,
            params KeyValuePair<Item, Vector2Int>[] items
        ) : this(width, height)
        {
            if (items == null)
                throw new ArgumentNullException();

            for (var index = 0; index < items.Length; index++)
            {
                var position = items[index].Value;
                var item = items[index].Key;

                InternalAdd(item, position);
            }
        }

        public Inventory(
            in int width,
            in int height,
            params Item[] items
        ) : this(width, height)
        {
            if (items == null)
                throw new ArgumentNullException();

            for (var index = 0; index < items.Length; index++)
            {
                var item = items[index];
                var position = IndexToPosition(index);
                InternalAdd(item, position);
            }
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<KeyValuePair<Item, Vector2Int>> items
        ) : this(width, height)
        {
            if (items == null)
                throw new ArgumentNullException();

            foreach (var keyValuePair in items)
            {
                var position = keyValuePair.Value;
                var item = keyValuePair.Key;
                InternalAdd(item, position);
            }
        }

        public Inventory(
            in int width,
            in int height,
            in IEnumerable<Item> items
        ) : this(width, height)
        {
            if (items == null)
                throw new ArgumentNullException();

            var index = 0;
            foreach (var item in items)
            {
                var position = IndexToPosition(index);
                InternalAdd(item, position);
                index++;
            }
        }

        /// <summary>
        /// Checks for adding an item on a specified position
        /// </summary>
        public bool CanAddItem(in Item item, in Vector2Int position)
        {
            if (position.y < 0 || position.x < 0)
                return false;

            if (!CanAddItem(item))
            {
                return false;
            }

            if (_width < item.Size.x + position.x)
            {
                return false;
            }

            if (_height < item.Size.y + position.y)
            {

                return false;
            }

            if (IsOccupiedIntersects(item, position))
            {

                return false;
            }

            return true;

        }
        /// <summary>
        /// Checks for adding an item on a free position
        /// </summary>
        public bool CanAddItem(in Item item)
        {
            if (item == null)
            {
                return false;
            }

            if (!item.IsCorrectSize())
                throw new ArgumentException();

            if (_cellsOccupiedItems.ContainsValue(item))
            {

                return false;
            }

            if (!FindFreePosition(item.Size, out var pos))
            {
                return false;
            }

            return true;
        }
        public bool CanAddItem(in Item item, in int posX, in int posY)
        {
            return CanAddItem(item, new Vector2Int(posX, posY));
        }

        private bool IsOccupiedIntersects(Item item, Vector2Int position, bool exceptSelf = false)
        {
            for (int i = 0; i < item.Size.x; i++)
            {
                for (int j = 0; j < item.Size.y; j++)
                {
                    var positionToCheck = position + new Vector2Int(i, j);
                    if (!_cellsOccupiedItems.TryGetValue(positionToCheck, out var itemAtPosition))
                    {
                        return true;
                    }

                    if (exceptSelf && item.Equals(itemAtPosition))
                    {
                        continue;
                    }

                    if (itemAtPosition != null)
                    {
                        return true;
                    }

                }
            }

            return false;
        }


        /// <summary>
        /// Adds an item on a specified position if not exists
        /// </summary>
        public bool AddItem(in Item item, in Vector2Int position)
        {
            if (!CanAddItem(item, position))
            {
                return false;
            }

            InternalAdd(item, position);

            OnAdded?.Invoke(item, position);

            return true;
        }

        public bool AddItem(in Item item, in int posX, in int posY)
        {
            return AddItem(item, new Vector2Int(posX, posY));
        }

        /// <summary>
        /// Adds an item on a free position
        /// </summary>
        public bool AddItem(in Item item)
        {
            if (!CanAddItem(item))
            {
                return false;
            }

            if (!FindFreePosition(item.Size, out var pos))
            {
                return false;
            }

            return AddItem(item, pos);
        }

        /// <summary>
        /// Returns a free position for a specified item
        /// </summary>
        public bool FindFreePosition(in Vector2Int size, out Vector2Int freePosition)
        {
            if (size.y <= 0 || size.x <= 0)
                throw new ArgumentOutOfRangeException();

            foreach (var keyValuePair in _cellsOccupiedItems)
            {
                    bool isSuccessPosition = true;
                    for (int i = 0; i < size.x; i++)
                    {
                        for (int j = 0; j < size.y; j++)
                        {
                            if (!IsFree(keyValuePair.Key + new Vector2Int(i, j)))
                            {
                                isSuccessPosition = false;
                                break;
                            }
                        }
                        if (!isSuccessPosition)
                        {
                            break;
                        }
                    }

                    if (!isSuccessPosition)
                    {
                        continue;
                    }
                    freePosition = keyValuePair.Key;
                    return true;
            }

            freePosition = default;
            return false;
        }

        /// <summary>
        /// Checks if a specified item exists
        /// </summary>
        public bool Contains(in Item item)
        {
            if (item == null)
            {
                return false;
            }
            return _itemsList.Contains(item);
        }

        /// <summary>
        /// Checks if a specified position is occupied
        /// </summary>
        public bool IsOccupied(in Vector2Int position)
            => _cellsOccupiedItems[position] != null;

        public bool IsOccupied(in int x, in int y)
            => _cellsOccupiedItems[new Vector2Int(x,y)] != null;

        /// <summary>
        /// Checks if a position is free
        /// </summary>
        public bool IsFree(in Vector2Int position)
        {
            if (position.x >= _width)
                return false;
            if (position.y >= _height)
                return false;

            return _cellsOccupiedItems[position] == null;
        }

        public bool IsFree(in int x, in int y)
            => IsFree(new Vector2Int(x, y));

        /// <summary>
        /// Removes a specified item if exists
        /// </summary>
        public bool RemoveItem(in Item item)
        {
            return RemoveItem(item, out var position);
        }

        public bool RemoveItem(in Item item, out Vector2Int position)
        {
            if (item == null || !_itemsList.Contains(item))
            {
                position = default;
                return false;
            }

            _itemsList.Remove(item);

            position = _itemPositions[item];
            _itemPositions.Remove(item);
            UpdateOccupiedCells();

                _countByNames[GetNameKey(item.Name)]--;

            OnRemoved?.Invoke(item, position);
            return true;
        }

        /// <summary>
        /// Returns an item at specified position 
        /// </summary>
        public Item GetItem(in Vector2Int position)
        {
            if (!_cellsOccupiedItems.ContainsKey(position))
                throw new IndexOutOfRangeException();

            var item = _cellsOccupiedItems[position];
            if (item == null)
                throw new NullReferenceException();

            return item;
        }

        public Item GetItem(in int x, in int y)
        {
            return GetItem(new Vector2Int(x, y));
        }

        public bool TryGetItem(in Vector2Int position, out Item item)
        {
            return TryGetItem(position.x, position.y, out item);
        }

        public bool TryGetItem(in int x, in int y, out Item item)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
            {
                item = null;
                return false;
            }

            if (_cellsOccupiedItems[new Vector2Int(x, y)] == null)
            {
                item = null;
                return false;
            }

            item = GetItem(x, y);
            return item != null;
        }

        /// <summary>
        /// Returns matrix positions of a specified item 
        /// </summary>
        public Vector2Int[] GetPositions(in Item item)
        {
            if(item == null)
                throw new NullReferenceException();

            if(!_itemPositions.ContainsKey(item))
                throw new KeyNotFoundException();

            var array = new Vector2Int[item.Size.x * item.Size.y];
            var position = _itemPositions[item];

            var index = 0;
            for (int i = 0; i < item.Size.x; i++)
            {
                for (int j = 0; j < item.Size.y; j++)
                {
                    array[index] = position + new Vector2Int(i, j);
                    index++;
                }
            }

            return array;
        }

        public bool TryGetPositions(in Item item, out Vector2Int[] positions)
        {
            try
            {
                 positions = GetPositions(item);
                 return true;
            }
            catch (Exception _)
            {
                positions = null;
                return false;
            }
        }

        /// <summary>
        /// Clears all inventory items
        /// </summary>
        public void Clear()
        {
            if(Count == 0)
                return;

            var keysToRemove = new List<Vector2Int>();
            foreach (var kvp in _cellsOccupiedItems)
            {
                if(_cellsOccupiedItems[kvp.Key] != null)
                    keysToRemove.Add(kvp.Key);
            }
            foreach (var key in keysToRemove)
            {
                _cellsOccupiedItems[key] = null;
            }

            _itemsList.Clear();
            _countByNames.Clear();
            _itemPositions.Clear();
            OnCleared?.Invoke();
        }

        /// <summary>
        /// Returns a count of items with a specified name
        /// </summary>
        public int GetItemCount(string name)
        {
            _countByNames.TryGetValue(GetNameKey(name), out var count);
            return count;
        }

        /// <summary>
        /// Moves a specified item to a target position if it exists
        /// </summary>
        public bool MoveItem(in Item item, in Vector2Int newPosition)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (newPosition.x < 0 || newPosition.x == _width || newPosition.y < 0 || newPosition.y == _height)
                return false;

            if (!_itemsList.Contains(item))
                return false;

            if (IsOccupiedIntersects(item, newPosition, true))
            {
                return false;
            }

            _itemPositions[item] = newPosition;
            UpdateOccupiedCells();
            OnMoved?.Invoke(item, newPosition);
            return true;
        }

        /// <summary>
        /// Reorganizes inventory space to make the free area uniform
        /// </summary>
        public void ReorganizeSpace()
        {
            //сохраняем изначальные позиции
            var savedPositions = new Dictionary<Item, Vector2Int>();
            foreach (var keyValuePair in _itemPositions)
                savedPositions.Add(keyValuePair.Key, keyValuePair.Value);

            //отделяем вещи от инвентаря
            _itemPositions.Clear();
            UpdateOccupiedCells();

            //сортируем по размеру
            var sorted = _itemsList.OrderByDescending(x=>x.Size.x * x.Size.y).ToList();

            //заполняем попорядку, вызываем событие, если позиция отличается
            foreach (var item in sorted)
            {
                if (FindFreePosition(item.Size, out var targetPosition))
                {
                    _itemPositions.Add(item, targetPosition);
                    UpdateOccupiedCells(item, targetPosition);

                    if(savedPositions[item] != targetPosition)
                        OnMoved?.Invoke(item, targetPosition);
                }
            }
        }

        /// <summary>
        /// Copies inventory items to a specified matrix
        /// </summary>
        public void CopyTo(in Item[,] matrix)
        {
            foreach (var (position, item) in _cellsOccupiedItems)
            {
                matrix[position.x, position.y] = item;
            }
        }

        public IEnumerator<Item> GetEnumerator()
        {
            return _itemsList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _itemsList.GetEnumerator();
        }

        private Vector2Int IndexToPosition(int index)
        {
            return new Vector2Int(_height / index, _width % index);
        }

        //не понадобился но почему бы и не оставить
        private int PositionToIndex(Vector2Int coordinates)
        {
            var index = coordinates.y * _width + coordinates.x;
            return index;
        }

        private void UpdateOccupiedCells()
        {
            _cellsOccupiedItems.Clear();

            InitCells();

            foreach (var keyValuePair in _itemPositions)
            {
                UpdateOccupiedCells(keyValuePair.Key, keyValuePair.Value);
            }
        }

        private void InitCells()
        {
            for (int j = 0; j < _height; j++)
            {
                for (int i = 0; i < _width; i++)
                {
                    _cellsOccupiedItems.Add(new Vector2Int(i, j), null);
                }
            }
        }

        private void UpdateOccupiedCells(Item item, Vector2Int coordinates)
        {
            for (var i = 0; i < item.Size.x; i++)
            {
                for (var j = 0; j < item.Size.y; j++)
                {
                    _cellsOccupiedItems[coordinates + new Vector2Int(i, j)] = item;
                }
            }
        }

        private void InternalAdd(Item item, Vector2Int position)
        {
            if (item != null)
            {
                _itemsList.Add(item);

                _countByNames.TryAdd(GetNameKey(item.Name), 0);
                _countByNames[GetNameKey(item.Name)]++;

                _itemPositions.TryAdd(item, position);
                UpdateOccupiedCells();
            }
        }

        private int GetNameKey(string name)
        {
            if (name == null)
                return -1;

            return name.GetHashCode();
        }
    }
}