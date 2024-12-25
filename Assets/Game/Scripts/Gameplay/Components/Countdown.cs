using System.Globalization;
using Newtonsoft.Json;
using SampleGame.Common.Data.ComponentData;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Countdown : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        public float Current { get; set; }

        ///Const
        [field: SerializeField]
        public float Duration { get; private set; }
        public override void SetSerializedData(string data) => Current = float.Parse(data, CultureInfo.InvariantCulture);

        public override string GetSerializedData() => Current.ToString(CultureInfo.InvariantCulture);

    }
}