using System.Collections;
using HealthSystem;
using UnityEngine;

namespace WeaponSystem.Weapons.Pickaxe
{
    public sealed class PickaxeWeapon : AWeaponBase
    {
        private static readonly int _hit = Animator.StringToHash("Hit");
        
        [SerializeField] private ThrownPickaxe thrownPickaxe;
        [SerializeField] private HistoryOnceDamageTrigger damageTrigger;
        [SerializeField] private float attackDuration;
        [SerializeField] private Animator hitPickaxeAnim;
        [SerializeField] private Animator slideAttackAnim;

        private Coroutine _damageTriggerCoroutine;

        protected override void Attack()
        {
            _damageTriggerCoroutine ??= StartCoroutine(DamageTriggerCoroutine());
            hitPickaxeAnim.SetTrigger(_hit);
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
            slideAttackAnim.gameObject.SetActive(true);
            slideAttackAnim.SetTrigger(_hit);

            yield return new WaitForSeconds(attackDuration);
            
            slideAttackAnim.SetTrigger(_hit);
            slideAttackAnim.gameObject.SetActive(false);
            _damageTriggerCoroutine = null;
            damageTrigger.gameObject.SetActive(false);
            damageTrigger.ResetHistory();
        }
    }
}