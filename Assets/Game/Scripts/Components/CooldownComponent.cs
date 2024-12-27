using System;
using UnityEngine;

namespace Game.Scripts.Components
{
    public class CooldownComponent : MonoBehaviour
    {
        [SerializeField] private float _cooldownTime;
        private float _timer;

        public bool IsReady => _timer<= 0f;

        public void Cooldown()
        {
            _timer = _cooldownTime;
        }

        public void Reset()
        {
            _timer = 0f;
        }

        private void Update()
        {
            if(IsReady)
                return;

            _timer -= Time.deltaTime;
        }
    }
}