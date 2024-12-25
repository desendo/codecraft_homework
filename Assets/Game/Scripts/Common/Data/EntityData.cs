using System;
using System.Collections.Generic;

namespace SampleGame.Common.Data
{
    [Serializable]
    public class EntityData
    {
        public int Id;
        public string Name;
        public SerializedVector3 Position;
        public SerializedVector3 Rotation;
        public Dictionary<string, string> Components = new();
    }
}