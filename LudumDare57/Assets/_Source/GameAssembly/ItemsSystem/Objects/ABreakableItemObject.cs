using HealthSystem;
using Services;
using UnityEngine;

// ReSharper disable InvertIf

namespace ItemsSystem.Objects
{
    public abstract class ABreakableItemObject : ItemObject, IHealth
    {
        [SerializeField] private LayerMask breakableByLayers;
        [SerializeField] private float breakVelocity;
        [SerializeField] private bool breakByHealthDamage = true;
        [SerializeField] private int maxHealth = 1;

        private int _health;

        protected override void Start()
        {
            base.Start();
            _health = maxHealth;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, breakableByLayers))
                return;

            if (IsRigidbodyHaveBreakForce(rb) ||
                other.TryGetComponent(out Rigidbody2D otherRb) && IsRigidbodyHaveBreakForce(otherRb))
                BreakInvoker();
        }

        protected bool IsRigidbodyHaveBreakForce(Rigidbody2D target)
        {
            return target.linearVelocityX > breakVelocity || target.linearVelocityY > breakVelocity ||
                   target.linearVelocityX < -breakVelocity || target.linearVelocityY < -breakVelocity;
        }

        private void BreakInvoker()
        {
            gameObject.SetActive(false);
            OnBreak();
        }
        
        protected abstract void OnBreak();

        public int GetHealth() => _health;

        public int GetMaxHealth() => maxHealth;

        public virtual bool CanGetDamage() => breakByHealthDamage;

        public void TakeDamage(int damage)
        {
            if (!CanGetDamage())
                return;

            _health = Mathf.Clamp(_health - damage, 0, maxHealth);
            
            if(_health == 0)
                BreakInvoker();
        }
    }
}