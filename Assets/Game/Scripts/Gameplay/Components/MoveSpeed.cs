using System.Globalization;
using Newtonsoft.Json;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class MoveSpeed : ComponentBase
    {
        ///Const
        [field: SerializeField]
        public float Current { get; private set; }
        public override void SetSerializedData(string data) => Current = float.Parse(data, NumberStyles.Float, CultureInfo.InvariantCulture);

        public override string GetSerializedData() => Current.ToString(CultureInfo.InvariantCulture);
    }
}