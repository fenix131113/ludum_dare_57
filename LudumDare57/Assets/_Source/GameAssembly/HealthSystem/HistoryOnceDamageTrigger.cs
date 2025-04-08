using System.Collections.Generic;
using Services;
using UnityEngine;

namespace HealthSystem
{
    public class HistoryOnceDamageTrigger : MonoBehaviour, IDamageApplier
    {
        [SerializeField] protected LayerMask targetLayers;
        [SerializeField] protected int damage;

        protected readonly HashSet<IHealth> Damaged = new();

        protected void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, targetLayers) ||
                !other.TryGetComponent(out IHealth health) || !Damaged.Add(health))
                return;
            
            health.TakeDamage(damage);
        }

        public void ResetHistory() => Damaged.Clear();

        public void SetDamageAmount(int newDamage) => damage = newDamage;

        public int GetDamageAmount() => damage;
    }
}