using System.Collections;
using Core;
using HealthSystem;
using UnityEngine;

namespace WeaponSystem
{
    public class ShooterBullet : ATriggerDamage, IPoolObject
    {
        private static readonly int _start = Animator.StringToHash("Start");
        
        [field: SerializeField] public override int Damage { get; protected set; }

        [SerializeField] private float speed;
        [SerializeField] private float shakeDuration;
        [SerializeField] private SpriteRenderer bulletRenderer;
        [SerializeField] private Animator bulletAnim;
        [SerializeField] private AnimationClip bulletStartAnim;

        private DictionaryObjectPool _pool;
        private bool _canMove;
        
        public void InitPool(DictionaryObjectPool pool) => _pool = pool;

        private void Update()
        {
            if(!_canMove)
                return;
            
            transform.position += transform.right * (speed * Time.deltaTime);
        }

        protected override void OnDamageGiven() => ReturnToPool();

        protected override void OnCollideNotWithTarget() => ReturnToPool();

        public void ActivateBullet()
        {
            bulletRenderer.sprite = null;
            _canMove = false;
            gameObject.SetActive(true);
            bulletAnim.SetTrigger(_start);
            StartCoroutine(SetStaticSpriteCoroutine());
        }

        public void ReturnToPool()
        {
            gameObject.SetActive(false);
            
            _pool.Push(this);
            StopAllCoroutines();
        }

        private IEnumerator SetStaticSpriteCoroutine()
        {
            yield return new WaitForSeconds(bulletStartAnim.length);
            
            CameraShaker.Instance.Shake(1.5f, 1.5f, shakeDuration);
            _canMove = true;
        }
    }
}