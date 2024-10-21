using UnityEngine;
using UnityEngine.Serialization;

namespace ShootEmUp
{
    public sealed class PlayerDeathObserver : MonoBehaviour
    {
        [SerializeField] private Player _character;

        private void Awake()
        {
            _character.OnDeath += _ => Time.timeScale = 0;
        }

    }
}