using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Game.Game.Scripts;
using UnityEngine;

namespace Game
{
    public class CoinView : MonoBehaviour
    {
        private TweenerCore<Vector3, Vector3, VectorOptions> _tween;

        public void StartMoving(Vector3 start, Vector3 finish, Action callback)
        {
            _tween?.Kill();
            transform.position = start;
            _tween = transform.DOMove(finish, Const.CoinFlyTime).SetEase(Ease.Linear).OnComplete(() => callback?.Invoke());
        }
    }
}