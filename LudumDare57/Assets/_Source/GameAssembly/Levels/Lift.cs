using Core;
using Interactable;
using Services;
using UnityEngine;
using VContainer;

namespace Levels
{
    public class Lift : MonoBehaviour
    {
        [SerializeField] private Lever exitLever;
        [SerializeField] private float animTime;
        [SerializeField] private bool loadShop = true;

        private GameState _gameState;

        [Inject]
        private void Construct(GameState gameState)
        {
            _gameState = gameState;
        }

        private void Start() => Bind();

        private void OnExitLeverPressed()
        {
            if (loadShop)
                FadeService.Instance.FadeIn(animTime, () => LevelControl.Instance.LoadShopLevel());
            else
                FadeService.Instance.FadeIn(animTime, () => LevelControl.Instance.LoadNextRandomLevel(true));
            
            _gameState.SetPlayerMovementPaused(true);
            _gameState.SetPlayerDamagePaused(true);
        }

        private void Bind() => exitLever.OnLeverPressed += OnExitLeverPressed; // Expose in lever
    }
}