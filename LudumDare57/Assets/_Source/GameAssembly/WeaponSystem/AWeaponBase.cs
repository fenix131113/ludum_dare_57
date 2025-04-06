using System.Collections;
using InputSystem;
using UnityEngine;
using VContainer;

namespace WeaponSystem
{
    public abstract class AWeaponBase : MonoBehaviour
    {
        [SerializeField] protected float attackCooldown;

        protected PlayerInput Input;
        protected bool CanShoot = true;

        [Inject]
        private void Construct(PlayerInput input) => Input = input;

        protected virtual void Start()
        {
            Bind();
        }

        private void OnDestroy() => Expose();

        protected abstract void Shoot();

        private void ShootInvoker()
        {
            if (!gameObject.activeSelf || !CanShoot)
                return;

            StartCoroutine(CooldownRoutine());
            Shoot();
        }

        protected virtual void Bind() => Input.OnShoot += ShootInvoker;

        protected virtual void Expose() => Input.OnShoot -= ShootInvoker;

        private IEnumerator CooldownRoutine()
        {
            CanShoot = false;

            yield return new WaitForSeconds(attackCooldown);

            CanShoot = true;
        }
    }
}