using Core;
using UnityEngine;
using VContainer;

namespace WeaponSystem.Weapons
{
    public class BasicFirearm : AAmmoWeapon
    {
        [SerializeField] protected Transform shootRotatePoint;
        [SerializeField] protected Transform shootSpawnPoint;
        [SerializeField] protected Bullet projectilePrefab;

        protected DictionaryObjectPool Pool;

        [Inject]
        private void BaseFirearmConstruct(DictionaryObjectPool pool) => Pool = pool;

        protected override void Attack()
        {
            base.Attack();
            
            if(CurrentAmmo == 0 || IsReloading)
                return;

            CurrentAmmo--;

            var projectile = GetBullet();
            
            projectile.transform.position = shootSpawnPoint.position + shootSpawnPoint.right * 0.3f;
            projectile.transform.rotation = shootRotatePoint.rotation; 
            projectile.ActivateBullet(GetDamageAmount());
            projectile.gameObject.SetActive(true);
            projectile.PoolInit(Pool);
            
            InvokeOnShoot();
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