using System;
using Game.Game.Scripts.App;
using Game.Gameplay;
using Modules.Common;
using Modules.DependencyInjection;
using Modules.Entities;
using UnityEngine;

namespace Game.Game.Scripts
{
    [DefaultExecutionOrder(-1000)]

    public class Di : MonoBehaviour
    {
        public static DependencyContainer Container { get; private set; }

        [SerializeField] private EntityWorld _entityWorld;
        [SerializeField] private EntityCatalog _entityCatalog;
        [SerializeField] private ControlsView _controlsView;
        private DependencyContainer _container;

        private void Awake()
        {
            _container = new DependencyContainer();
            Container = _container;
            InstallDependencies();
        }

        private void InstallDependencies()
        {
            _container.Add<SignalBus>();

            _container.Add(_entityWorld);
            _container.Add(_entityCatalog);
            _container.AddInject<ControlsPresenter>();
            _container.AddInject(_controlsView);

            _container.Add<ServerSaveLoadHandler>();
            _container.AddInject<SaveDataService>();
            _container.AddInject<GameStateService>();

            _container.AddInject<SaveLoadController>();
        }
    }
}