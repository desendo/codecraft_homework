using System;
using Game.Game.Scripts;
using Game.Game.Scripts.App;
using Modules.Common;

namespace Game.Gameplay
{
    public sealed class ControlsPresenter : IControlsPresenter
    {
        public void Save(Action<bool, int> callback)
        {
            Di.Container.Get<SignalBus>().Fire(new PresenterSignals.SaveRequest(callback));
        }

        public void Load(string versionText, Action<bool, int> callback)
        {
            Di.Container.Get<SignalBus>().Fire(new PresenterSignals.LoadRequest(versionText, callback));
        }
    }
}