using System;
using UnityEngine;

namespace Interactable
{
    public sealed class Lever : MonoBehaviour, IInteractableObject
    {
        private bool _pressed;

        public event Action OnLeverPressed;
        
        public void Interact()
        {
            _pressed = true;
            OnLeverPressed?.Invoke();
        }

        public bool CanInteract() => !_pressed;
    }
}