using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Team : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        [ComponentValue]
        public TeamType Type { get; set; }

    }
}