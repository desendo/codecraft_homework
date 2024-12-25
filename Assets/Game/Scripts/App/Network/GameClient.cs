using Cysharp.Threading.Tasks;
using UnityEngine.Networking;

namespace Game.Game.Scripts.App
{
    public static class GameClient
    {
        private static string baseUrl = "http://127.0.0.1:8888";

        public static async UniTask<bool> SaveGameAsync(string gameState, int version)
        {
            string url = $"{baseUrl}/save?version={version}";

            var request = new UnityWebRequest(url, "PUT");
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(gameState);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "text/plain");

            await request.SendWebRequest();
            return request.result == UnityWebRequest.Result.Success;
        }

        public static async UniTask<string> LoadGameAsync(int version)
        {
            var url = $"{baseUrl}/load?version={version}";

            var request = UnityWebRequest.Get(url);

            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                return request.downloadHandler.text;
            }
            else
            {
                return null;
            }
        }
    }
}