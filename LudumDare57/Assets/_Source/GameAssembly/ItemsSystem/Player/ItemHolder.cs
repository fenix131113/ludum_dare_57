using System;
using DG.Tweening;
using InputSystem;
using UnityEngine;
using VContainer;

namespace ItemsSystem.Player
{
    public class ItemHolder : MonoBehaviour
    {
        public CarryObject CurrentObject { get; private set; }

        public event Action<CarryObject> OnCurrentItemChanged;
        
        private PlayerInput _input;
        private bool _isTakingItem;

        [Inject]
        private void Construct(PlayerInput input) => _input = input;

        private void OnInteract()
        {
            if (!CurrentObject || CurrentObject.ItemType != ItemType.OBJECT)
                return;

            // Prevent double interaction
            if (_isTakingItem)
            {
                _isTakingItem = false;
                return;
            }
            
            CurrentObject.ResetObject();
            CurrentObject = null;
        }
        
        private void Start() => Bind();

        private void OnDestroy() => Expose();
        
        public void TakeItem(CarryObject carryObject)
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