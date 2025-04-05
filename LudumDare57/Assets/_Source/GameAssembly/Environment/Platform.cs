using System.Collections;
using UnityEngine;

namespace Environment
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private float reactivationTime;
        [SerializeField] private LayerMask playerLayer;

        private Collider2D _platformCollider;
        private bool _isDeactivated;

        private void Awake() => _platformCollider = GetComponent<Collider2D>();

        public void DeactivateCollision()
        {
            if (_isDeactivated)
                return;
    
            _isDeactivated = true;
            _platformCollider.excludeLayers += playerLayer;
            StartCoroutine(ReactivatePlatformCollision());
        }

        private IEnumerator ReactivatePlatformCollision()
        {
            yield return new WaitForSeconds(reactivationTime);
            _isDeactivated = false;
            _platformCollider.excludeLayers -= playerLayer;
        }
    }
}