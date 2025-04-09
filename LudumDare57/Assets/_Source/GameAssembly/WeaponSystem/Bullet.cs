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
        private bool _damageGiven;

        private void Update() => transform.position += transform.right * (speed * Time.deltaTime);

        protected override void OnTriggerEnter2D(Collider2D other)
        {
            ReturnToPool();
            base.OnTriggerEnter2D(other);
        }

        public void ActivateBullet(int overrideDamage = 0)
        {
            SetDamageAmount(overrideDamage == 0 ? Damage : overrideDamage);
            _damageGiven = false;
            gameObject.SetActive(true);
        }

        protected override void OnDamageGiven() => _damageGiven = true;

        protected override bool CanApplyDamage() => !_damageGiven;

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