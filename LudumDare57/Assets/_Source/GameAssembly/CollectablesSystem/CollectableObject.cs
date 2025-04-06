using Services;
using UnityEngine;

namespace CollectablesSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class CollectableObject : MonoBehaviour
    {
        [SerializeField] protected LayerMask interactLayers;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayers))
                return;
            
            Collect();
        }

        protected abstract void Collect();
    }
}