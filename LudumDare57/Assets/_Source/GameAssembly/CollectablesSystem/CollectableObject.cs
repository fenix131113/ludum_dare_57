using Services;
using UnityEngine;

namespace CollectablesSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class CollectableObject : MonoBehaviour
    {
        [SerializeField] protected LayerMask interactLayers;
        [SerializeField] private AudioClip[] collectSounds;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayers))
                return;
            
            if(collectSounds.Length > 0)
                SoundPlayService.Instance.PlaySound(collectSounds[Random.Range(0, collectSounds.Length)]);
            
            Collect();
        }

        protected abstract void Collect();
    }
}