using System;
using Core;
using Interactable;
using UnityEngine;
using VContainer;

namespace Levels.Shop
{
    public class Trader : MonoBehaviour, IInteractableObject
    {
        private GameState _gameState;
        
        public event Action OnInteract;
        
        [Inject]
        private void Construct(GameState gameState) => _gameState = gameState;

        public void Interact()
        {
            _gameState.SetGameCycleBlocked(true);
            OnInteract?.Invoke();
        }

        public bool CanInteract() => true;
    }
}