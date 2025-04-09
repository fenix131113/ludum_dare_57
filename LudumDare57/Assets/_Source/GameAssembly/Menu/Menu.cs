using Core.Ui;
using Levels;
using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private SpriteButton spriteButton;

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
            Bind();
        }

        private void OnDestroy() => Expose();

        private void LoadGame() => LevelControl.Instance.LoadNextRandomLevel(false);

        private void Bind() => spriteButton.OnClick += LoadGame;

        private void Expose() => spriteButton.OnClick -= LoadGame;
    }
}