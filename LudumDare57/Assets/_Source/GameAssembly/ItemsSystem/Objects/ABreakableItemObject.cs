using Services;
using UnityEngine;

// ReSharper disable InvertIf

namespace ItemsSystem.Objects
{
    public abstract class ABreakableItemObject : ItemObject
    {
        [SerializeField] private LayerMask breakableByLayers;
        [SerializeField] private float breakVelocity;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, breakableByLayers))
                return;

            if (IsRigidbodyHaveBreakForce(rb) ||
                other.TryGetComponent(out Rigidbody2D otherRb) && IsRigidbodyHaveBreakForce(otherRb))
            {
                gameObject.SetActive(false);
                OnBreak();
            }
        }

        protected bool IsRigidbodyHaveBreakForce(Rigidbody2D target)
        {
            return target.linearVelocityX > breakVelocity || target.linearVelocityY > breakVelocity ||
                   target.linearVelocityX < -breakVelocity || target.linearVelocityY < -breakVelocity;
        }

        protected abstract void OnBreak();
    }
}