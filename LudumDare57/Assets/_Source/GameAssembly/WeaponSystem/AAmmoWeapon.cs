using UnityEngine;

namespace WeaponSystem
{
    public abstract class AAmmoWeapon : AWeaponBase
    {
        [SerializeField] protected int maxAmmo;
        [SerializeField] protected float reloadTime;

        protected int CurrentAmmo;

        private float _reloadTimer;

        public bool IsReloading { get; private set; }

        public float ReloadProgress => _reloadTimer / reloadTime;

        protected override void Start()
        {
            base.Start();
            ResetReloadTimer();
            RestoreAmmo();
        }

        protected virtual void Update()
        {
            if (IsReloading)
            {
                if (_reloadTimer > 0)
                    _reloadTimer -= Time.deltaTime;
                else
                {
                    OnReloadComplete();
                    ResetReloadTimer();
                }
            }
        }


        protected virtual void OnReloadComplete() => RestoreAmmo();

        protected void ResetReloadTimer()
        {
            IsReloading = false;
            _reloadTimer = reloadTime;
        }

        private void Reload() => IsReloading = true;

        public void CancelReload() => ResetReloadTimer();
        public void RestoreAmmo() => CurrentAmmo = maxAmmo;

        protected override void Bind()
        {
            base.Bind();
            BindAmmoWeapon();
        }

        protected override void Expose()
        {
            base.Expose();
            ExposeAmmoWeapon();
        }

        private void BindAmmoWeapon() => Input.OnReload += Reload;

        private void ExposeAmmoWeapon() => Input.OnReload += Reload;
    }
}