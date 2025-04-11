using System.Collections;
using HealthSystem;
using ItemsSystem.Objects;
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
        private WeaponObject _pickaxeWeapon;

        protected override void Start()
        {
            _pickaxeWeapon = GetComponent<WeaponObject>();
            base.Start();
        }

        protected override void Attack()
        {
            if(!CanAttackCondition())
                return;
            
            _damageTriggerCoroutine ??= StartCoroutine(DamageTriggerCoroutine());
            hitPickaxeAnim.SetTrigger(_hit);
        }

        protected override bool CanAttackCondition() => !thrownPickaxe.Stuck && !thrownPickaxe.Thrown;

        private void ThrowPickaxe() => thrownPickaxe.Throw();

        public void ResetRotation()
        {
            transform.parent.rotation = Quaternion.identity;
            StopAllCoroutines();
            slideAttackAnim.gameObject.SetActive(false);
        }

        protected override void Bind()
        {
            base.Bind();
            Input.OnAim += ThrowPickaxe;
            _pickaxeWeapon.OnWeaponReset += ResetRotation;
        }

        protected override void Expose()
        {
            base.Expose();
            Input.OnAim -= ThrowPickaxe;
            _pickaxeWeapon.OnWeaponReset -= ResetRotation;
        }

        private IEnumerator DamageTriggerCoroutine()
        {
            damageTrigger.SetDamageAmount(GetDamageAmount());
            damageTrigger.gameObject.SetActive(true);
            slideAttackAnim.gameObject.SetActive(true);
            slideAttackAnim.SetTrigger(_hit);

            yield return new WaitForSeconds(attackDuration);
            
            slideAttackAnim.gameObject.SetActive(false);
            _damageTriggerCoroutine = null;
            damageTrigger.gameObject.SetActive(false);
            damageTrigger.ResetHistory();
        }
    }
}