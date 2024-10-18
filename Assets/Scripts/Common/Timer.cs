using System;
using UnityEngine;

namespace ShootEmUp
{
    public class Timer : MonoBehaviour
    {
        private float _maxTime;
        private float _time;
        private bool _autoRestart;
        private bool _triggered;
        private bool _started;

        public event Action OnTime;
        public void Setup(float maxTime, bool autoRestart = false)
        {
            _maxTime = maxTime;
            _autoRestart = autoRestart;
        }
        public void StartTimer()
        {
            _started = true;
        }

        public void StopTimer()
        {
            _started = false;
        }

        public void RestartTimer()
        {
            _time = 0;
            _triggered = false;
            _started = true;
        }

        private void Update()
        {
            if(!_started)
                return;

            _time += Time.deltaTime;
            if (_time >= _maxTime && !_triggered)
            {
                OnTime?.Invoke();
                _triggered = true;
            }

            if (_triggered && _autoRestart)
            {
                RestartTimer();
            }
        }


        private void OnDisable()
        {
            _time = 0;
        }
    }
}