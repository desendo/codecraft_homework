using System;
using UnityEngine;

namespace ConverterModule.Scripts
{
    public class CycleTimer
    {
        private readonly float _cycleTime;
        private float _runTime;
        private int _cyclesCompleted;

        public bool Started { get; private set; }

        //свойство нужно для теста но может пригодиться и в реальной жизни
        public float Runtime => _runTime;

        public event Action OnCycle;

        /// <summary>
        /// циклический таймер, адаптивный вне зависимости от величины dt.
        /// можем подавать время по кусочком или целиком, событие OnCycle отработает корректно
        /// </summary>
        /// <param name="cycleTime"></param>
        /// <exception cref="ArgumentOutOfRangeException">при неположительном значении времени одного цикла</exception>
        public CycleTimer(float cycleTime)
        {
            if (cycleTime <= 0f)
                throw new ArgumentOutOfRangeException();

            _cycleTime = cycleTime;
        }

        public void Start()
        {
            Started = true;
        }

        public void Stop()
        {
            _cyclesCompleted = 0;
            _runTime = 0;
            Started = false;
        }

        //как показали тесты такая логика расчета дает точные результаты с минимумом кода
        public void Update(float dt)
        {
            if(!Started)
                return;

            _runTime += dt;
            var totalCycles = Mathf.FloorToInt(_runTime / _cycleTime);

            if (totalCycles <= _cyclesCompleted) return;

            var newCyclesCount = totalCycles - _cyclesCompleted;

            for (var i = 0; i < newCyclesCount; i++)
                OnCycle?.Invoke();

            _cyclesCompleted += newCyclesCount;
        }
    }
}