using Core;
using UnityEngine;
using VContainer;

namespace WeaponSystem
{
    public class Pistol : AWeaponBase
    {
        [SerializeField] private Transform shootRotatePoint;
        [SerializeField] private Transform shootSpawnPoint;
        [SerializeField] private Bullet projectilePrefab;

        private DictionaryObjectPool _pool;

        [Inject]
        private void PistolConstruct(DictionaryObjectPool pool) => _pool = pool;

        protected override void Shoot()
        {
            var bulletExists = _pool.TryPop<Bullet>(out var bullet);

            var projectile = bulletExists
                ? bullet
                : Instantiate(projectilePrefab);

            projectile!.transform.position = shootSpawnPoint.position; 
            projectile!.transform.rotation = shootRotatePoint.rotation; 
            projectile!.gameObject.SetActive(true);
            projectile!.PoolInit(_pool);
        }
    }
}