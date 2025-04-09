using Core;
using InputSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Menu.InGame
{
    public class InGameMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menu;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button exitButton;

        private PlayerInput _playerInput;
        private GameState _gameState;
        
        [Inject]
        private void Construct(PlayerInput playerInput, GameState gameState)
        {
            _playerInput = playerInput;
            _gameState = gameState;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void ToggleMenu()
        {
            if(_gameState.PlayerMovementPaused)
                return;
            
            if(menu.activeSelf)
                CloseMenu();
            else
                OpenMenu();
        }

        private void OpenMenu()
        {
            _gameState.SetGamePaused(true);
            menu.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0f;
        }

        private void CloseMenu()
        {
            _gameState.SetGamePaused(false);
            menu.SetActive(false);
            Cursor.visible = false;
            Time.timeScale = 1f;
        }

        private void LoadMenu() => SceneManager.LoadScene(0);

        private void Bind()
        {
            _playerInput.OnMenu += ToggleMenu;
            continueButton.onClick.AddListener(ToggleMenu);
            exitButton.onClick.AddListener(LoadMenu);
        }

        private void Expose()
        {
            _playerInput.OnMenu -= ToggleMenu;
            continueButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();
        }
    }
}