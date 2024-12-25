using Newtonsoft.Json;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Health : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;

        public override void SetSerializedData(string data) => Current = int.Parse(data);

        public override string GetSerializedData() => Current.ToString();
    }
}