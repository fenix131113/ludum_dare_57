using UnityEngine;

namespace HealthSystem
{
    public class DamageTrigger : ATriggerDamage
    {
        [SerializeField] protected bool damageOnce;

        public override int Damage { get; protected set; }

        protected bool CanDamage = true;

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            if (!CanDamage || !gameObject.activeSelf)
                return;

            if (damageOnce)
                CanDamage = false;
            
            base.OnTriggerEnter2D(other);
        }

        public void ResetDamageState() => CanDamage = true;
    }
}