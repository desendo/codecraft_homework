using Game.Game.Scripts.App;
using UnityEngine;

namespace SampleGame.Gameplay
{
    public abstract class ComponentBase : MonoBehaviour
    {

        public virtual void SavePrepare(IComponentSaveLoadVisitor saveLoadVisitor)
        {

        }

        public virtual void Init(IComponentSaveLoadVisitor saveLoadVisitor)
        {
        }

    }

}