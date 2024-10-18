using UnityEngine;

namespace ShootEmUp
{
    public interface IUnit
    {
        public bool IsPlayer { get; }
        void Damage(int damage);
    }
}