using UnityEngine;

namespace WeaponSystem.Weapons
{
    public sealed class Shotgun : BasicFirearm
    {
        [SerializeField] private float spreadDegrees;
        [SerializeField] private int fractionAmount;

        protected override void Shoot()
        {
            if (CurrentAmmo == 0 || IsReloading)
                return;

            CurrentAmmo--;

            for (var i = 0; i < fractionAmount; i++)
            {
                var projectile = GetBullet();
                var spreadPart = spreadDegrees / 2;
                var rotation = Quaternion.Euler(0, 0, shootRotatePoint.eulerAngles.z + Random.Range(-spreadPart, spreadPart));

                projectile.transform.position = shootSpawnPoint.position;
                projectile.transform.rotation = rotation;
                projectile.gameObject.SetActive(true);
                projectile.PoolInit(Pool);
            }
        }

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