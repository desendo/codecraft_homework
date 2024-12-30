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

    }
}