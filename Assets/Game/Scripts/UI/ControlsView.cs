using System.Linq;
using Modules.DependencyInjection;
using Modules.Entities;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gameplay
{
    //Don't modify
    public sealed class ControlsView : MonoBehaviour
    {
        private IControlsPresenter _presenter;

        [ShowInInspector]
        private EntityCatalog _catalog;

        private string _versionText = string.Empty;

        [Inject]
        public void Construct(EntityCatalog entityCatalog, IControlsPresenter presenter)
        {
            _catalog = entityCatalog;
            _presenter = presenter;
        }

        private void OnGUI()
        {
            this.DrawSaveButton();
            this.DrawLoadButton();
            this.DrawVersionText();
        }

        private void DrawLoadButton()
        {
            Rect button1Rect = new Rect(10, 50, 100, 30);
            if (GUI.Button(button1Rect, "Load Game"))
                this.OnLoadClicked();
        }

        private void DrawVersionText()
        {
            // Показываем подсказку, если текст пустой и поле неактивное


            Rect inputFieldRect = new Rect(120, 50, 75, 30);
            var textFieldStyle = new GUIStyle(GUI.skin.textField)
            {
                alignment = TextAnchor.MiddleCenter
            };

            string filteredText = GUI.TextField(inputFieldRect, _versionText, 3, textFieldStyle);
            if (string.IsNullOrEmpty(filteredText))
            {
                GUI.color = Color.gray;
                GUI.Label(inputFieldRect, "version", textFieldStyle);
                GUI.color = Color.white;
                
                _versionText = string.Empty;
            }
            else
            {
                _versionText = string.Concat(filteredText.Where(char.IsDigit));
            }
        }

        private void DrawSaveButton()
        {
            Rect button2Rect = new Rect(10, 10, 100, 30);
            if (GUI.Button(button2Rect, "Save Game"))
                this.OnSaveClicked();
        }

        private void OnSaveClicked() => _presenter.Save(this.OnSaveResult);
        private void OnLoadClicked() => _presenter.Load(_versionText, this.OnLoadResult);

        private void OnSaveResult(bool success, int version)
        {
            Debug.Log(success
                ? $"<color=green><b>Sucessfully saved version: {version}</b></color>"
                : "<color=red><b>Saving failed!</b></color>");
        }

        private void OnLoadResult(bool success, int version)
        {
            Debug.Log(success
                ? $"<color=green><b>Sucessfully loaded version: {version}</b></color>"
                : "<color=red><b>Loading failed!</b></color>");
        }
    }
}