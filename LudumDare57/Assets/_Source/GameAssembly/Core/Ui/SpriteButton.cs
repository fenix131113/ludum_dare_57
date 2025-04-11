using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Ui
{
    public class SpriteButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite spriteDefault;
        [SerializeField] private Sprite spriteHover;
        
        public event Action OnClick;

        private void Start() => buttonImage.sprite = spriteDefault;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.pointerEnter != gameObject)
                return;
            
            OnClick?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(eventData.pointerEnter != gameObject)
                return;
            
            buttonImage.sprite = spriteHover;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(eventData.pointerEnter != gameObject)
                return;

            buttonImage.sprite = spriteDefault;
        }
    }
}