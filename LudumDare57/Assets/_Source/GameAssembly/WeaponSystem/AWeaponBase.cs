using InputSystem;
using UnityEngine;
using VContainer;

namespace WeaponSystem
{
    public abstract class AWeaponBase : MonoBehaviour
    {
        protected PlayerInput Input;
        
        [Inject]
        private void Construct(PlayerInput input) => Input = input;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        protected abstract void Shoot();

        private void ShootInvoker()
        {
            if(!gameObject.activeSelf)
                return;
            
            Shoot();
        }

        private void Bind() => Input.OnShoot += ShootInvoker;

        private void Expose() => Input.OnShoot -= ShootInvoker;
    }
}