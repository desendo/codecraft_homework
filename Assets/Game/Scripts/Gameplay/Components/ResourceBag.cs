using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ResourceBag : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        [ComponentValue]
        public ResourceType Type { get; set; }

        ///Variable
        [field: SerializeField]
        [ComponentValue]
        public int Current { get; set; }

        ///Const
        [field: SerializeField]
        public int Capacity { get; set; }

    }
}