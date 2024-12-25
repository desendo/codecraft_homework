using System;
using Game.Game.Scripts;
using Game.Game.Scripts.App;
using UnityEngine;

namespace Game.UI
{
    public class LoadingView : MonoBehaviour, IDisposable
    {
        [SerializeField] private GameObject _panel;
        private IReadOnlyGameStateService _loadingService;

        private void Awake()
        {
            _loadingService = Di.Container.Get<IReadOnlyGameStateService>();
            _loadingService.OnLoading += HandleLoadingServiceOnLoading;
            HandleLoadingServiceOnLoading();
        }

        private void HandleLoadingServiceOnLoading()
        {
            _panel.SetActive(_loadingService.IsLoading);
        }

        public void Dispose()
        {
            _loadingService.OnLoading -= HandleLoadingServiceOnLoading;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
