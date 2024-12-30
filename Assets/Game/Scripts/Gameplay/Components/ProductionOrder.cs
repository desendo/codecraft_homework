using System.Collections.Generic;
using Game.Game.Scripts.App;
using Modules.Entities;
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

        [ComponentValue]
        public List<string> QueueEntityIds { get; private set; } = new List<string>();

        public override void SavePrepare(IComponentSaveLoadVisitor saveLoadVisitor)
        {
            QueueEntityIds.Clear();
            foreach (var entityConfig in _queue)
            {
                QueueEntityIds.Add(entityConfig.Name);
            }
        }

        public override void Init(IComponentSaveLoadVisitor saveLoadVisitor)
        {
            _queue.Clear();
            foreach (var queueEntityId in QueueEntityIds)
            {
                if(saveLoadVisitor.Catalog.FindConfig(queueEntityId, out var cfg))
                    _queue.Add(cfg);
            }
        }
    }
}