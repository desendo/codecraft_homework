using System;
using System.Collections.Generic;
using Modules.Common;
using Modules.Entities;
using SampleGame.Common.Data;
using SampleGame.Gameplay;
using UnityEngine;

namespace Game.Game.Scripts.App
{
    public class SaveLoadController : IDisposable
    {
        private readonly IGameStateService _gameStateService;
        private readonly EntityWorld _entityWorld;
        private readonly SaveDataService _saveDataService;

        private readonly List<IDisposable> _subscriptions = new ();
        private readonly EntityCatalog _entityCatalog;

        public SaveLoadController(ISignalBus signalBus, EntityCatalog entityCatalog, IGameStateService gameStateService, EntityWorld entityWorld,
            SaveDataService saveDataService)
        {
            _entityCatalog = entityCatalog;
            _gameStateService = gameStateService;
            _entityWorld = entityWorld;
            _saveDataService = saveDataService;
            signalBus.Subscribe<PresenterSignals.LoadRequest>(HandleLoadRequest).AddTo(_subscriptions);
            signalBus.Subscribe<PresenterSignals.SaveRequest>(HandleSaveRequest).AddTo(_subscriptions);
        }

        private async void HandleSaveRequest(PresenterSignals.SaveRequest obj)
        {

            _gameStateService.SetIsLoading(true);
            var entityDataList = new List<EntityData>();
            foreach (var entity in _entityWorld.GetAll())
            {
                var entityData = new EntityData();

                entityData.Id = entity.Id;
                entityData.Name = entity.Name;
                entityData.Position = entity.transform.position;
                entityData.Rotation = entity.transform.rotation.eulerAngles;
                var components = entity.GetComponents<ComponentBase>();
                foreach (var component in components)
                {
                    if(component.SkipSerialization)
                        continue;

                    var typeString = component.GetType().ToString();
                    entityData.Components.Add(typeString, component.GetSerializedData());
                }
                entityDataList.Add(entityData);
            }

            var result = await _saveDataService.Save<List<EntityData>>(entityDataList);

            obj.Callback.Invoke(result.Item1, result.Item2);
            _gameStateService.SetIsLoading(false);
        }

        private async void HandleLoadRequest(PresenterSignals.LoadRequest request)
        {
            if (!int.TryParse(request.VersionText, out var version))
            {
                request.Callback.Invoke(false, -1);
                Debug.LogError($"cant parse version string to int :{request.VersionText}");
                return;
            }
            _gameStateService.SetIsLoading(true);
            var result = await _saveDataService.Load<List<EntityData>>(version);
            if (result == null)
            {
                request.Callback.Invoke(false, -1);
                Debug.LogError($"load failed");
                return;
            }

            _entityWorld.DestroyAll();

            foreach (var entityData in result)
            {
                if (_entityCatalog.FindConfig(entityData.Name, out var config))
                {
                    var entity = _entityWorld.Spawn(config, entityData.Position, entityData.Rotation);

                    SetEntityData(entityData, entity);
                }
                else
                {
                    Debug.LogError($"cant find config by name {entityData.Name}");
                }
            }
            foreach (var entity in _entityWorld.GetAll())
            {
                InitializeEntity(entity);
            }

            request.Callback.Invoke(true, version);

            _gameStateService.SetIsLoading(false);

        }

        private void InitializeEntity(Entity entity)
        {
            foreach (var componentBase in entity.GetComponents<ComponentBase>())
            {
                componentBase.EntityRelatedInitialize(_entityCatalog, _entityWorld);
            }
        }

        private static void SetEntityData(EntityData entityData, Entity entity)
        {
            foreach (var (key, value) in entityData.Components)
            {
                var type = Type.GetType(key);
                if (type == null)
                {
                    Debug.LogError($"cant find component type for name {key}");
                    continue;
                }

                if (entity.TryGetComponent(type, out var component))
                {
                    var componentBase = component as ComponentBase;
                    if (componentBase == null)
                        Debug.LogError($"component {component} is not {typeof(ComponentBase)}");
                    else
                        componentBase.SetSerializedData(value);
                }
                else
                {
                    Debug.LogError($"cant find component {key} entity {entity.Name}");
                }
            }
        }

        public void Dispose()
        {
            _subscriptions.DisposeAndClear();
        }
    }
}