using System;
using UnityEngine;

namespace WeaponSystem
{
    public abstract class AAmmoWeapon : AWeaponBase
    {
        [field: SerializeField] public int MaxAmmo { get; protected set; }

        [SerializeField] protected float reloadTime;

        public int CurrentAmmo
        {
            get => _currentAmmo;

            protected set
            {
                _currentAmmo = value;
                OnAmmoChanged?.Invoke();
            }
        }

        private int _currentAmmo;
        private float _reloadTimer;

        public bool IsReloading { get; private set; }

        public float ReloadProgress => 1f - _reloadTimer / reloadTime;

        public event Action OnShoot;
        public event Action OnAmmoChanged;

        protected override void Start()
        {
            base.Start();
            ResetReloadTimer();
            RestoreAmmo();
        }

        protected override void Update()
        {
            base.Update();

            if (!IsReloading)
                return;

            if (_reloadTimer > 0)
                _reloadTimer -= Time.deltaTime;
            else
            {
                OnReloadComplete();
                ResetReloadTimer();
            }
        }

        protected override void Attack()
        {
            if(!CanAttackCondition())
                return;
            
            if (CurrentAmmo == 0 && !IsReloading)
                Reload();
        }

        protected void InvokeOnShoot() => OnShoot?.Invoke();

        protected virtual void OnReloadComplete() => RestoreAmmo();

        protected void ResetReloadTimer()
        {
            IsReloading = false;
            _reloadTimer = reloadTime;
        }

        protected void Reload()
        {
            if (CurrentAmmo >= MaxAmmo || IsReloading)
                return;

            ResetReloadTimer();
            IsReloading = true;
        }

        public void CancelReload() => ResetReloadTimer();

        public void RestoreAmmo() => CurrentAmmo = MaxAmmo;

        public void ChangeMaxAmmo(int newMax) => MaxAmmo = newMax;

        public void SetReloadTimeMultiplier(float multiplier) => reloadTime *= multiplier;

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