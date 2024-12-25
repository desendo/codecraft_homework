
using Cysharp.Threading.Tasks;
using Game.Game.Scripts.App;
using Newtonsoft.Json;
using UnityEngine;

namespace Game.Game.Scripts
{
    public interface ISaveHandler
    {
        UniTask<bool> Save(string jsonString, int version);
        UniTask<string> Load(int version);
    }

    public class ServerSaveLoadHandler : ISaveHandler
    {
        public async UniTask<bool> Save(string jsonString, int version)
        {
            return await GameClient.SaveGameAsync(jsonString, version);
        }

        public async UniTask<string> Load(int version)
        {
            return await GameClient.LoadGameAsync(version);
        }
    }

    public class SaveDataService
    {
        private readonly ISaveHandler _saveHandler;

        private const string CurrentVersionKey = "CURRENT_VERSION";
        private int _currentVersion = 0;
        public SaveDataService(ISaveHandler saveHandler)
        {
            _currentVersion = PlayerPrefs.GetInt(CurrentVersionKey, 0);
            _saveHandler = saveHandler;
        }
        public async UniTask<(bool, int)> Save<T>(T dataObject)
        {
            var result = await _saveHandler.Save(JsonConvert.SerializeObject(dataObject), _currentVersion + 1);
            if (result)
            {
                PlayerPrefs.SetInt(CurrentVersionKey, _currentVersion);
                PlayerPrefs.Save();
                _currentVersion++;
            }

            return (result ,_currentVersion);

        }

        public async UniTask<T> Load<T>(int version) where T: class
        {
            var result = await _saveHandler.Load(version);

            if (string.IsNullOrEmpty(result))
                return null;

            return JsonConvert.DeserializeObject<T>(result);
        }
    }
}