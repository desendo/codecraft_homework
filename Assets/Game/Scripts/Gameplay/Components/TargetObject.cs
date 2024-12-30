using Game.Game.Scripts.App;
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

        [ComponentValue]
        public int ValueId { get; private set; } = -1;

        public override void SavePrepare(IComponentSaveLoadVisitor saveLoadVisitor)
        {
            ValueId = Value == null ? -1 : Value.Id;
        }

        public override void Init(IComponentSaveLoadVisitor saveLoadVisitor)
        {
            if (ValueId != -1)
            {
                Value = saveLoadVisitor.World.Get(ValueId);
            }
        }
    }
}