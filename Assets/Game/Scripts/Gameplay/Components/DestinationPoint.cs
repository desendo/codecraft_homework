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
        public Vector3 Value { get; set; }

        public override string GetSerializedData()
        {
            return JsonConvert.SerializeObject((SerializedVector3)Value);
        }

        public override void SetSerializedData(string data)
        {
            Value = JsonConvert.DeserializeObject<SerializedVector3>(data);
        }
    }
}