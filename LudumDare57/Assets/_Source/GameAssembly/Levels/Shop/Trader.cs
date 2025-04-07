using System;
using Core;
using Interactable;
using UnityEngine;
using Upgrades;
using VContainer;

namespace Levels.Shop
{
    public sealed class Trader : MonoBehaviour, IInteractableObject
    {
        [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
        
        private GameState _gameState;
        
        public event Action OnInteract;
        
        [Inject]
        private void Construct(GameState gameState) => _gameState = gameState;

        public void Interact()
        {
            if(_gameState.GameCycleBlocked)
                return;
            
            _gameState.SetGameCycleBlocked(true);
            OnInteract?.Invoke();
        }

        public bool CanInteract() => true;
    }
}