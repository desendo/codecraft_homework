
using Modules.Entities;
using UnityEngine;

namespace SampleGame.Gameplay
{
    public abstract class ComponentBase : MonoBehaviour
    {
        public virtual bool SkipSerialization { get; } = false;
        public virtual string GetSerializedData()
        {
            return string.Empty;
        }

        public virtual void SetSerializedData(string data)
        {
        }

        public virtual void EntityRelatedInitialize(EntityCatalog entityCatalog, EntityWorld entityWorld)
        {
        }
    }

}