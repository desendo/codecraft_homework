
using UnityEngine;
using UnityEngine.Events;
namespace Game.Scripts.Components
{
   public class TrampolineComponent : MonoBehaviour
   {
      [SerializeField] private float _force;
      [SerializeField] private UnityEvent _onAddForce;
      private void OnTriggerEnter2D(Collider2D other)
      {
         other.attachedRigidbody.AddForce(transform.up* _force);
         _onAddForce?.Invoke();
      }
   }
}
