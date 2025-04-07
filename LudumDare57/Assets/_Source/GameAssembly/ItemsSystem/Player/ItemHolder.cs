using System;
using Core;
using DG.Tweening;
using InputSystem;
using ItemsSystem.Objects;
using UnityEngine;
using VContainer;

namespace ItemsSystem.Player
{
    public sealed class ItemHolder : MonoBehaviour
    {
        public ACarryObject CurrentObject { get; private set; }

        public event Action<ACarryObject> OnCurrentItemChanged;
        
        private PlayerInput _input;
        private GameState _gameState;
        private bool _isTakingItem;

        [Inject]
        private void Construct(PlayerInput input, GameState gameState)
        {
            _input = input;
            _gameState = gameState;
        }

        private void OnInteract()
        {
            if (!CurrentObject || CurrentObject.ItemType != ItemType.OBJECT || _gameState.GameCycleBlocked)
                return;

            // Prevent double interaction
            if (_isTakingItem)
            {
                _isTakingItem = false;
                return;
            }
            
            CurrentObject.ResetObject();
            CurrentObject = null;
            OnCurrentItemChanged?.Invoke(null);
        }
        
        private void Start() => Bind();

        private void OnDestroy() => Expose();
        
        public void TakeItem(ACarryObject carryObject)
        {
            CurrentObject?.ResetObject();
            
            _isTakingItem = true;
            CurrentObject = carryObject;
            OnCurrentItemChanged?.Invoke(carryObject);
        }

        private void Bind() => _input.OnInteract += OnInteract;

        private void Expose() => _input.OnInteract -= OnInteract;
    }
}