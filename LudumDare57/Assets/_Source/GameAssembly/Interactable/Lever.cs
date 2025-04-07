using System;
using UnityEngine;

namespace Interactable
{
    public sealed class Lever : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite activateSprite;
        
        private bool _pressed;

        public event Action OnLeverPressed;
        
        public void Interact()
        {
            _pressed = true;
            spriteRenderer.sprite = activateSprite;
            OnLeverPressed?.Invoke();
        }

        public bool CanInteract() => !_pressed; // TODO: Add missions depends
    }
}