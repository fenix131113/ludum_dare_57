using Core;
using UnityEngine;

namespace WeaponSystem.Weapons
{
    public sealed class Shotgun : BasicFirearm
    {
        [SerializeField] private float spreadDegrees;
        [SerializeField] private int fractionAmount;

        protected override void Attack()
        {
            if (CurrentAmmo == 0 && !IsReloading)
            {
                Reload();
                return;
            }
            
            if(IsReloading)
                return;

            CameraShaker.Instance.Shake(1.5f, 1.5f,0.15f);
            CurrentAmmo--;

            for (var i = 0; i < fractionAmount; i++)
            {
                var projectile = GetBullet();
                var spreadPart = spreadDegrees / 2;
                var rotation = Quaternion.Euler(0, 0,
                    shootRotatePoint.eulerAngles.z + Random.Range(-spreadPart, spreadPart));

                projectile.transform.position = shootSpawnPoint.position;
                projectile.transform.rotation = rotation;
                projectile.ActivateBullet(GetDamageAmount());
                projectile.gameObject.SetActive(true);
                projectile.PoolInit(Pool);
            }

            InvokeOnShoot();
        }

        public void SetFractionAmount(int amount) => fractionAmount = amount;

        private Bullet GetBullet()
        {
            var bulletExists = Pool.TryPop<Bullet>(out var bullet);

            var projectile = bulletExists
                ? bullet
                : Instantiate(projectilePrefab);

            return projectile;
        }
    }
}