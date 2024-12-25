using System.Collections.Generic;
using Modules.Entities;
using Newtonsoft.Json;
using SampleGame.Common.Data.ComponentData;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ProductionOrder : ComponentBase
    {
        ///Variable
        [SerializeField]
        private List<EntityConfig> _queue;

        public IReadOnlyList<EntityConfig> Queue
        {
            get { return _queue; }
            set { _queue = new List<EntityConfig>(value); }
        }

        private ProductionOrderData _data = new ProductionOrderData();

        public override void SetSerializedData(string data)
        {
            _data = JsonConvert.DeserializeObject<ProductionOrderData>(data);
        }

        public override string GetSerializedData()
        {
            _data.QueueEntityIds.Clear();
            foreach (var entityConfig in _queue)
            {
                _data.QueueEntityIds.Add(entityConfig.Name);
            }
            return JsonConvert.SerializeObject(_data);
        }

        public override void EntityRelatedInitialize(EntityCatalog entityCatalog, EntityWorld entityWorld)
        {
            _queue.Clear();
            foreach (var id in _data.QueueEntityIds)
            {
                if (entityCatalog.FindConfig(id, out var config))
                {
                    _queue.Add(config);
                }
                else
                {
                    Debug.LogError($"cant find entity config for id:{id}");
                }
            }
        }
    }
}