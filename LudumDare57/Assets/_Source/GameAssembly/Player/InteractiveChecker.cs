﻿using System;
using Core;
using InputSystem;
using Interactable;
using ItemsSystem;
using ItemsSystem.Objects;
using ItemsSystem.Player;
using Services;
using UnityEngine;
using VContainer;

namespace Player
{
    public sealed class InteractiveChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayers;

        private IInteractableObject _currentInteractable;
        private PlayerInput _input;
        private ItemHolder _itemHolder;
        private GameState _gameState;

        public event Action<GameObject> OnInteractTargetChanged;

        [Inject]
        private void Construct(PlayerInput input, ItemHolder itemHolder, GameState gameState)
        {
            _input = input;
            _itemHolder = itemHolder;
            _gameState = gameState;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetLayers) ||
                !other.TryGetComponent(out IInteractableObject interactable) || !interactable.CanInteract()
                || (_itemHolder.CurrentObject && _itemHolder.CurrentObject.ItemType == ItemType.OBJECT))
                return;

            _currentInteractable = interactable;
            OnInteractTargetChanged?.Invoke(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetLayers) ||
                !other.TryGetComponent(out IInteractableObject interactable) || interactable != _currentInteractable)
                return;

            _currentInteractable = null;
            OnInteractTargetChanged?.Invoke(null);
        }

        private void OnInteract()
        {
            if(_gameState.GamePaused)
                return;
            
            switch (_currentInteractable)
            {
                case null:
                    return; 
                case ACarryObject carry:
                    _itemHolder.TakeItem(carry);

                    //TODO: Check new interactable in player interaction zone (╯°□°）╯︵ ┻━┻
                    break;
                default:
                    _currentInteractable.Interact();
                    break;
            }

            if(_currentInteractable == null)
                return;
            
            _currentInteractable = null;
            OnInteractTargetChanged?.Invoke(null); //TODO: Do after new interactable check 
        }


        private void Bind() => _input.OnInteract += OnInteract;

        private void Expose() => _input.OnInteract -= OnInteract;
    }
}