using System.Collections;
using Core;
using UnityEngine;

namespace WeaponSystem
{
    public class Bullet : MonoBehaviour, IPoolObject
    {
        [SerializeField] private float speed;
        [SerializeField] private float deadTime;

        private DictionaryObjectPool _pool;
        
        private void Update() => transform.position += transform.right * (speed * Time.deltaTime);

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