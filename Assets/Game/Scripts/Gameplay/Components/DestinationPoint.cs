using Newtonsoft.Json;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class DestinationPoint : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        [ComponentValue]
        public Vector3 Value { get; set; }

    }
}