using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Game.Scripts.Components
{
    public class AddForceComponent : MonoBehaviour
    {
        [SerializeField] private float _force;
        [SerializeField] private CooldownComponent _cooldown;
        [SerializeField] private Transform _applicationPoint;
        [SerializeField] private float _radius = 1f;
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;
        [SerializeField] private GameObject _ignore;

        [SerializeField] private UnityEvent _onAddForce;

        private void Awake()
        {
        }

        
        public void TryForce()
        {
            if (_cooldown.IsReady)
            {
                var dir = (_to.position - _from.position).normalized;

                var effectedColliders = new Collider2D[10];
                var collidersCount = Physics2D.OverlapCircleNonAlloc(_applicationPoint.position, _radius, effectedColliders);
                if (collidersCount > 0)
                {
                    foreach (var effectedCollider in effectedColliders)
                    {
                        if (effectedCollider != null && effectedCollider.attachedRigidbody != null)
                        {
                            if(effectedCollider.gameObject == _ignore)
                                continue;

                            effectedCollider.attachedRigidbody.AddForceAtPosition( dir * _force, _applicationPoint.position);
                        }
                    }
                }
                _onAddForce.Invoke();
                _cooldown.Cooldown();
            }
        }
    }
}
