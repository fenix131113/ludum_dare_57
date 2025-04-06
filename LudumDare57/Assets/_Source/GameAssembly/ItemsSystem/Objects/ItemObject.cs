using DG.Tweening;
using Player;
using UnityEngine;
using VContainer;

namespace ItemsSystem.Objects
{
    public class ItemObject : ACarryObject
    {
        protected Rigidbody2D PlayerRb;
        protected Transform StartParent;

        [Inject]
        private void Construct(PlayerMovement playerMovement) => PlayerRb = playerMovement.GetComponent<Rigidbody2D>();

        protected virtual void Start() => StartParent = transform.parent;

        public override void ResetObject()
        {
            DOTween.Kill(gameObject);
            transform.parent = StartParent;
            rb.linearVelocity = PlayerRb.linearVelocity;

            EnablePhysics();
        }
    }
}