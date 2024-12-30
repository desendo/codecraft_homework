using System;
using System.Collections.Generic;
using System.Reflection;
using Modules.Common;
using Modules.Entities;
using Newtonsoft.Json;
using SampleGame.Common.Data;
using SampleGame.Common.JsonConverters;
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
        private readonly SaveLoadVisitor _saveLoadVisitor;
        private readonly JsonSerializerSettings _jsonSettings;

        public SaveLoadController(ISignalBus signalBus, EntityCatalog entityCatalog, IGameStateService gameStateService, EntityWorld entityWorld,
            SaveDataService saveDataService)
        {
            _entityCatalog = entityCatalog;
            _gameStateService = gameStateService;
            _entityWorld = entityWorld;
            _saveDataService = saveDataService;
            signalBus.Subscribe<PresenterSignals.LoadRequest>(HandleLoadRequest).AddTo(_subscriptions);
            signalBus.Subscribe<PresenterSignals.SaveRequest>(HandleSaveRequest).AddTo(_subscriptions);

            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.Converters.Add(new Vector3Converter());

            _saveLoadVisitor = new SaveLoadVisitor(_entityWorld, _entityCatalog);
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
                    component.SavePrepare(_saveLoadVisitor);
                    var componentType = component.GetType();
                    var typeString = componentType.ToString();
                    foreach (var property in componentType.GetProperties())
                    {
                        if (property.GetCustomAttribute<ComponentValueAttribute>() != null)
                        {
                            var key = $"{typeString}:{property.Name}";
                            entityData.Components.Add(key, JsonConvert.SerializeObject(property.GetValue(component), _jsonSettings));
                        }

                    }
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
                    var entity = _entityWorld.Spawn(config, entityData.Position, entityData.Rotation, entityData.Id);

                    foreach (var (key,value) in entityData.Components)
                    {

                        var parts = key.Split(':');
                        if (parts.Length != 2)

                            throw new ArgumentException("Invalid key format.");

                        var componentName = parts[0];
                        var fieldKey = parts[1];

                        var componentType = Type.GetType(componentName);
                        if (componentType == null || !typeof(MonoBehaviour).IsAssignableFrom(componentType))
                            throw new InvalidOperationException($"Component type {componentName} not found or invalid.");

                        if(entity.GetComponent(componentType) == null)
                            entity.gameObject.AddComponent(componentType);
                        var component = entity.GetComponent(componentType);

                        var field = componentType.GetProperty(fieldKey);
                        if (field == null)
                            throw new InvalidOperationException($"Field {fieldKey} not found on component {componentName}.");

                        field.SetValue(component,JsonConvert.DeserializeObject(value, field.PropertyType, _jsonSettings));

                    }

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
                componentBase.Init(_saveLoadVisitor);
            }
        }


        public void Dispose()
        {
            _subscriptions.DisposeAndClear();
        }
    }
}