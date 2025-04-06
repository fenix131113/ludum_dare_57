using Services;
using UnityEngine;

namespace HealthSystem
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class ATriggerDamage : MonoBehaviour, IDamageApplier
    {
        [SerializeField] protected LayerMask damageTargetLayers;
        
        public abstract int Damage { get; protected set; }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if(!LayerService.CheckLayersEquality(other.gameObject.layer, damageTargetLayers) || !other.TryGetComponent<IHealth>(out var health))
                return;
            
            health.TakeDamage(Damage);
            OnDamageGiven();
        }

        protected virtual void OnDamageGiven()
        {
        }

        public void SetDamageAmount(int newDamage) => Damage = newDamage;
        public int GetDamageAmount() => Damage;
    }
}