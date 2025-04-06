using System.Collections;
using Core;
using HealthSystem;
using UnityEngine;

namespace WeaponSystem
{
    public class Bullet : ATriggerDamage, IPoolObject
    {
        [field: SerializeField] public override int Damage { get; protected set; }

        [SerializeField] private float speed;
        [SerializeField] private float deadTime;

        private DictionaryObjectPool _pool;

        private void Update() => transform.position += transform.right * (speed * Time.deltaTime);

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.gameObject.name);
            ReturnToPool();
            base.OnTriggerEnter2D(other);
        }

        public void ActivateBullet(int overrideDamage = 0)
        {
            SetDamageAmount(overrideDamage == 0 ? Damage : overrideDamage);
            gameObject.SetActive(true);
        }

        public void PoolInit(DictionaryObjectPool pool)
        {
            _pool = pool;
            StartCoroutine(DespawnCooldown());
        }

        public void ReturnToPool()
        {
            StopAllCoroutines();

            _pool.Push(this);
            gameObject.SetActive(false);
        }

        private IEnumerator DespawnCooldown()
        {
            yield return new WaitForSeconds(deadTime);

            ReturnToPool();
        }
    }
}