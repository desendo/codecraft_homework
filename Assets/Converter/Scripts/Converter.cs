using System;
using UnityEngine;

namespace ConverterModule.Scripts
{
    public class Converter
    {
        private readonly Storage _loadingArea;
        private readonly Storage _shippingArea;
        private readonly Processor _processor;

        public event Action<int> BurnedInput;
        public event Action<int> BurnedOutput;
        public event Action<int> OnLoadingAreaChanged;
        public event Action<int> OnShippingAreaChanged;
        public int LoadingAreaCount { get; private set; }
        public int ShippingAreaCount { get; private set; }
        public bool IsOn { get; private set; }


        /// <param name="loadingAreaCapacity">вместимость входного хранилища</param>
        /// <param name="shippingAreaCapacity">вместимость выходного хранилища</param>
        /// <param name="inputCount">входных единиц в обработку</param>
        /// <param name="outputCount">выходных единиц после обработки</param>
        /// <param name="workCycleTime"></param>
        /// <exception cref="ArgumentOutOfRangeException">неправильные значения не благославляются</exception>
        public Converter(int loadingAreaCapacity, int shippingAreaCapacity, int inputCount, int outputCount, float workCycleTime)
        {
            if (inputCount > loadingAreaCapacity)
                throw new ArgumentOutOfRangeException();

            if (outputCount > shippingAreaCapacity)
                throw new ArgumentOutOfRangeException();

            _loadingArea = new Storage(loadingAreaCapacity);
            _shippingArea = new Storage(shippingAreaCapacity);

            _loadingArea.OnChange += i =>
            {
                LoadingAreaCount = i;
                OnLoadingAreaChanged?.Invoke(i);
            };

            _shippingArea.OnChange += i =>
            {
                ShippingAreaCount = i;
                OnShippingAreaChanged?.Invoke(i);
            };

            _processor = new Processor(inputCount, outputCount, LoadingAreaTakeHandler, ShippingAreaPutHandler, ProcessorStoppedHandler, workCycleTime);
        }

        private void ProcessorStoppedHandler(int count)
        {
            _loadingArea.Put( count, out var change);
            if(change > 0)
                BurnedInput?.Invoke(change);
        }

        private void ShippingAreaPutHandler(int count)
        {
            _shippingArea.Put(count, out var change);
            if(change > 0)
                BurnedOutput?.Invoke(change);
        }

        private bool LoadingAreaTakeHandler(int input)
        {
            return _loadingArea.Take(input);
        }

        public void Start()
        {
            _processor.PowerOn();
            IsOn = true;
        }
        public void Stop()
        {
            _processor.PowerOff();
            IsOn = false;
        }

        public void Update(float dt)
        {
            _processor.Update(dt);
        }

        public int Load(int count)
        {
            _loadingArea.Put(count, out var change);
            return change;
        }

        public bool Ship(int count)
        {
            return _shippingArea.Take(count);
        }
    }
}
