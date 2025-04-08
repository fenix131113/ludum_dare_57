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

        private GameState _gameState;
        
        [Inject]
        private void Construct(GameState gameState)
        {
            _gameState = gameState;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnExitLeverPressed()
        {
            FadeService.Instance.FadeIn(animTime, () => LevelControl.Instance.LoadNextRandomLevel());
            _gameState.SetGameCycleBlocked(true);
        }

        private void Bind() => exitLever.OnLeverPressed += OnExitLeverPressed;

        private void Expose() => exitLever.OnLeverPressed -= OnExitLeverPressed;
    }
}