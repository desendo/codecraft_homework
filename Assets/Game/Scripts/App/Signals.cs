using System;

namespace Game.Game.Scripts.App
{
    public class PresenterSignals
    {
        public class SaveRequest
        {
            public Action<bool, int> Callback { get; }

            public SaveRequest(Action<bool, int> callback)
            {
                Callback = callback;
            }
        }

        public class LoadRequest
        {
            public string VersionText { get; }
            public Action<bool, int> Callback { get; }

            public LoadRequest(string versionText, Action<bool, int> callback)
            {
                VersionText = versionText;
                Callback = callback;
            }
        }
    }
}