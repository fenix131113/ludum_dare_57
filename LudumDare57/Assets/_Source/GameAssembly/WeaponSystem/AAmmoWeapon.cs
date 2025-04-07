using UnityEngine;

namespace WeaponSystem
{
    public abstract class AAmmoWeapon : AWeaponBase
    {
        [field: SerializeField] public int MaxAmmo { get; protected set; }
        
        [SerializeField] protected float reloadTime;

        protected int CurrentAmmo;

        private float _reloadTimer;
        private float _reloadTimeMultiplier;

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

        protected override void Attack()
        {
            if(CurrentAmmo == 0 && !IsReloading)
                Reload();
        }

        protected virtual void OnReloadComplete() => RestoreAmmo();

        protected void ResetReloadTimer()
        {
            IsReloading = false;
            _reloadTimer = reloadTime * _reloadTimeMultiplier;
        }

        protected void Reload()
        {
            if (CurrentAmmo < MaxAmmo)
                IsReloading = true;
        }

        public void CancelReload() => ResetReloadTimer();
        
        public void RestoreAmmo() => CurrentAmmo = MaxAmmo;
        
        public void ChangeMaxAmmo(int newMax) => MaxAmmo = newMax;
        
        public void SetReloadTimeMultiplier(float multiplier) => _reloadTimeMultiplier = multiplier;

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