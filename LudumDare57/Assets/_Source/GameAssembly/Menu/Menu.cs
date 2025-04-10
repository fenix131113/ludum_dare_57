using Core.Ui;
using Levels;
using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private SpriteButton loadGameButton;

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
            Bind();
        }

        private void OnDestroy() => Expose();

        private void LoadGame() => LevelControl.Instance.LoadNextRandomLevel(false);

        private void Bind() => loadGameButton.OnClick += LoadGame;

        private void Expose() => loadGameButton.OnClick -= LoadGame;
    }
}