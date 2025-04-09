using System;
using Levels.Quests;
using UnityEngine;

namespace Interactable
{
    public sealed class Lever : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite activateSprite;
        [SerializeField] private bool questsDepend;
        [SerializeField] private QuestsManager questsManager;
        
        private bool _pressed;

        public event Action OnLeverPressed;

        private void OnDestroy() => OnLeverPressed = null;

        public void Interact()
        {
            _pressed = true;
            spriteRenderer.sprite = activateSprite;
            OnLeverPressed?.Invoke();
        }

        public bool CanInteract()
        {
            if(questsDepend && !questsManager.IsQuestComplete)
                return false;
            
            return !_pressed;
        }
    }
}