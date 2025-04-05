using DG.Tweening;
using UnityEngine;

namespace ItemsSystem
{
    public class ItemObject : CarryObject
    {
        [SerializeField] private Rigidbody2D playerRb;
        
        private Transform _startParent;

        private void Start() => _startParent = transform.parent;

        public override void ResetObject()
        {
            DOTween.Kill(gameObject);
            transform.parent = _startParent;
            rb.linearVelocity = playerRb.linearVelocity;
            
            EnablePhysics();
        }
    }
}