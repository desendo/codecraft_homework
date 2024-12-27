using UnityEngine;
namespace Game.Scripts.Components
{
    public class LookComponent : MonoBehaviour
    {
        [SerializeField] private Transform[] _viewHandlers;
        private Vector2 _lookDirection = Vector2.right;

        public Vector2 LookDirection => _lookDirection;
        public void SetLookRight(bool isRight)
        {
            foreach (var viewHandler in _viewHandlers)
            {
                var localScale = viewHandler.localScale;
                localScale = new Vector3(Mathf.Abs(localScale.x) * (isRight? 1:-1) , localScale.y, localScale.z);
                viewHandler.localScale = localScale;
            }
            _lookDirection = isRight ? Vector2.right : Vector2.left;
        }
    }
}
