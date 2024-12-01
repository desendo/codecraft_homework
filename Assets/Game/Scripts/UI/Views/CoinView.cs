using UnityEngine;

namespace Game
{
    public class CoinView : MonoBehaviour
    {
        public void StartMoving(Vector3 start, Vector3 finish)
        {
            transform.position = start;
        }
    }
}