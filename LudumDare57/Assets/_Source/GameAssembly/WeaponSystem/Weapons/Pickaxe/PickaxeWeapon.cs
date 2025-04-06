using System.Collections;
using HealthSystem;
using UnityEngine;

namespace WeaponSystem.Weapons.Pickaxe
{
    public sealed class PickaxeWeapon : AWeaponBase
    {
        [SerializeField] private ThrownPickaxe thrownPickaxe;
        [SerializeField] private HistoryOnceDamageTrigger damageTrigger;
        [SerializeField] private float attackDuration;

        private Coroutine _damageTriggerCoroutine;

        protected override void Attack()
        {
            _damageTriggerCoroutine ??= StartCoroutine(DamageTriggerCoroutine());
        }

        protected override bool CanAttackCondition() => !thrownPickaxe.Stuck && !thrownPickaxe.Thrown;

        private void ThrowPickaxe() => thrownPickaxe.Throw();

        protected override void Bind()
        {
            base.Bind();
            Input.OnAim += ThrowPickaxe;
        }

        protected override void Expose()
        {
            base.Expose();
            Input.OnAim -= ThrowPickaxe;
        }

        private IEnumerator DamageTriggerCoroutine()
        {
            damageTrigger.SetDamageAmount(GetDamageAmount());
            damageTrigger.gameObject.SetActive(true);

            yield return new WaitForSeconds(attackDuration);
            
            _damageTriggerCoroutine = null;
            damageTrigger.gameObject.SetActive(false);
            damageTrigger.ResetHistory();
        }
    }
}