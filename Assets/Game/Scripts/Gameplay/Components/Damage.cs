using Newtonsoft.Json;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Damage : ComponentBase
    {
        public override bool SkipSerialization { get; } = true;

        ///Const
        [field: SerializeField]
        public int Value { get; private set; } = 10;

    }
}