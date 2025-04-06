using System.Collections;
using HealthSystem;
using InputSystem;
using UnityEngine;
using VContainer;

namespace WeaponSystem
{
    public abstract class AWeaponBase : MonoBehaviour, IDamageApplier
    {
        [SerializeField] private int damage;
        [SerializeField] protected float attackCooldown;

        protected PlayerInput Input;
        protected bool CanAttack = true;

        private Coroutine _cooldownCoroutine;

        [Inject]
        private void Construct(PlayerInput input) => Input = input;

        protected virtual void Start() => Bind();

        private void OnDestroy() => Expose();

        protected abstract void Attack();

        private void ShootInvoker()
        {
            if (!gameObject.activeSelf || !CanAttack || !CanAttackCondition())
                return;
            
            _cooldownCoroutine = StartCoroutine(CooldownRoutine());
            Attack();
        }

        public void ResetCooldownInstantly()
        {
            if (_cooldownCoroutine != null)
                StopCoroutine(_cooldownCoroutine);

            CanAttack = true;
        }

        public void SetDamageAmount(int newDamage) => damage = newDamage;
        
        public int GetDamageAmount() => damage;

        protected virtual bool CanAttackCondition() => CanAttack;

        protected virtual void Bind() => Input.OnShoot += ShootInvoker;

        protected virtual void Expose() => Input.OnShoot -= ShootInvoker;

        private IEnumerator CooldownRoutine()
        {
            CanAttack = false;

            yield return new WaitForSeconds(attackCooldown);

            CanAttack = true;
        }
    }
}