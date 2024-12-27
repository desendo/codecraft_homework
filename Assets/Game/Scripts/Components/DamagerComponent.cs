using UnityEngine;
using UnityEngine.Events;
namespace Game.Scripts.Components
{
   public class DamagerComponent : MonoBehaviour
   {
      [SerializeField] private int _damage;
      [SerializeField] private UnityEvent _onDamage;

      private void OnTriggerEnter2D(Collider2D other)
      {
         var otherHealth = other.gameObject.GetComponent<HealthComponent>();
         if (otherHealth != null)
         {
            otherHealth.Damage(_damage);
            _onDamage.Invoke();
         }
      }

      private void OnCollisionEnter2D(Collision2D other)
      {
         var otherHealth = other.gameObject.GetComponent<HealthComponent>();
         if (otherHealth != null)
         {
            otherHealth.Damage(_damage);
            _onDamage.Invoke();
         }
      }
   }
}
