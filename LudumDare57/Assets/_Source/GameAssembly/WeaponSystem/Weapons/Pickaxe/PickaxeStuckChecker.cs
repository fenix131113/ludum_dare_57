using System.Collections;
using Services;
using UnityEngine;

namespace WeaponSystem.Weapons.Pickaxe
{
    public class PickaxeStuckChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask interactLayers;
        [SerializeField] private ThrownPickaxe thrownPickaxe;
        [SerializeField] private bool bounceCollider;
        [SerializeField] private float bounceForceMin;
        [SerializeField] private float bounceForceMax;
        [SerializeField] private PickaxeStuckChecker stuckChecker;
        [SerializeField] private PickaxePickup pickupTrigger;
        
        private Rigidbody2D _thrownPickaxeRb;

        private void Awake() => _thrownPickaxeRb = thrownPickaxe.GetComponent<Rigidbody2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, interactLayers) || !gameObject.activeInHierarchy)
                return;

            if (bounceCollider)
            {
                StartCoroutine(StuckCheckerCoroutine());
                
                _thrownPickaxeRb.linearVelocity = Vector2.zero;
                
                var contactPoint = other.ClosestPoint(transform.position);
                var bounceDirection = ((Vector2)transform.position - contactPoint).normalized;
                _thrownPickaxeRb.AddForce(bounceDirection * Random.Range(bounceForceMin, bounceForceMax), ForceMode2D.Impulse);
            }
            else
            {
                pickupTrigger.gameObject.SetActive(true);
                thrownPickaxe.StuckPickaxe();
            }
        }

        private IEnumerator StuckCheckerCoroutine()
        {
            stuckChecker.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.1f);
            
            stuckChecker.gameObject.SetActive(true);
        }
    }
}