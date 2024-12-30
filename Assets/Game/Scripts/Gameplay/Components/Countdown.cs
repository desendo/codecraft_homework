
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Countdown : ComponentBase
    {
        ///Variable
        [field: SerializeField]
        [ComponentValue]
        public float Current { get; set; }

        ///Const
        [field: SerializeField]
        public float Duration { get; private set; }


    }
}