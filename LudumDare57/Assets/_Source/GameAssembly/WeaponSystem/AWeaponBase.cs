using System.Collections;
using Core;
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
        protected GameState GameState;
        protected bool CanAttack = true;

        private float _attackCooldownTimer;

        [Inject]
        private void Construct(PlayerInput input, GameState gameState)
        {
            Input = input;
            GameState = gameState;
        }

        protected virtual void Start() => Bind();

         protected virtual void Update()
        {
            if (!CanAttack && _attackCooldownTimer > 0)
                _attackCooldownTimer -= Time.deltaTime;
            else if(_attackCooldownTimer <= 0)
                ResetCooldownInstantly();
        }

        private void OnDestroy() => Expose();

        protected abstract void Attack();

        private void ShootInvoker()
        {
            if (!gameObject.activeSelf || !CanAttack || !CanAttackCondition())
                return;

            CanAttack = false;
            _attackCooldownTimer = attackCooldown;
            Attack();
        }

        public void ResetCooldownInstantly() => CanAttack = true;

        public void SetDamageAmount(int newDamage) => damage = newDamage;
        
        public int GetDamageAmount() => damage;

        protected virtual bool CanAttackCondition() => CanAttack && !(GameState.PlayerMovementPaused || GameState.GamePaused);

        protected virtual void Bind() => Input.OnShoot += ShootInvoker;

        protected virtual void Expose() => Input.OnShoot -= ShootInvoker;
    }
}