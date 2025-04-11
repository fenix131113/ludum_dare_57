using Core.Ui;
using DG.Tweening;
using Levels;
using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private SpriteButton loadGameButton;
        [SerializeField] private SpriteButton settingButton;
        [SerializeField] private CanvasGroup settingsPanel;
        [SerializeField] private float settingsPanelAnimDuration = 0.3f;

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 1f;
            Bind();
        }

        private void OnDestroy() => Expose();

        private void LoadGame() => LevelControl.Instance.LoadNextRandomLevel(false);

        private void SwitchSettings()
        {
            if (settingsPanel.gameObject.activeSelf)
            {
                settingsPanel.DOFade(0f, settingsPanelAnimDuration).onComplete +=
                    () => settingsPanel.gameObject.SetActive(false);
            }
            else
            {
                settingsPanel.alpha = 0;
                settingsPanel.gameObject.SetActive(true);
                settingsPanel.DOFade(1f, settingsPanelAnimDuration);
            }
        }

        private void Bind()
        {
            loadGameButton.OnClick += LoadGame;
            settingButton.OnClick += SwitchSettings;
        }

        private void Expose()
        {
            loadGameButton.OnClick -= LoadGame;
            settingButton.OnClick -= SwitchSettings;
        }
    }
}