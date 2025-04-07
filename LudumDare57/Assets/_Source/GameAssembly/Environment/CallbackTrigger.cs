using System;
using Services;
using UnityEngine;

namespace Environment
{
    public class CallbackTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayers;

        public event Action<GameObject> OnTargetEntered;
        public event Action<GameObject> OnTargetExit;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, targetLayers))
                return;
            
            OnTargetEntered?.Invoke(other.gameObject);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, targetLayers))
                return;
            
            OnTargetExit?.Invoke(other.gameObject);
        }
    }
}