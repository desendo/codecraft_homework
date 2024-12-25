using Modules.Entities;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class TargetObject : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        public Entity Value { get; set; }

        private int _valueId;

        public override void SetSerializedData(string data) => _valueId = int.Parse(data);

        public override string GetSerializedData()
        {
            if (Value == null)
                _valueId = -1;
            else
            {
                _valueId = Value.Id;
            }
            return _valueId.ToString();
        }

        public override void EntityRelatedInitialize(EntityCatalog entityCatalog, EntityWorld entityWorld)
        {
            if (_valueId == -1)
            {
                Value = null;
                return;
            }

            if (!entityWorld.Has(_valueId))
            {
                Debug.LogError($"cant find entity id:{_valueId}");
                return;
            }

            Value = entityWorld.Get(_valueId);
        }
    }
}