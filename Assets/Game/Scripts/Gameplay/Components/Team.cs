using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        public TeamType Type { get; set; }

        public override void SetSerializedData(string data) => Type = (TeamType)int.Parse(data);
        public override string GetSerializedData() => ((int)Type).ToString();
    }
}