using Newtonsoft.Json;
using SampleGame.Common;
using SampleGame.Common.Data.ComponentData;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ResourceBag : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        public ResourceType Type { get; set; }

        ///Variable
        [field: SerializeField]
        public int Current { get; set; }

        ///Const
        [field: SerializeField]
        public int Capacity { get; set; }

        private ResourceBagData _data = new ResourceBagData();
        public override string GetSerializedData()
        {
            _data.Current = Current;
            _data.Type = Type;
            return JsonConvert.SerializeObject(_data);
        }

        public override void SetSerializedData(string data)
        {
            _data = JsonConvert.DeserializeObject<ResourceBagData>(data);
            Current = _data.Current;
            Type = _data.Type;
        }
    }
}