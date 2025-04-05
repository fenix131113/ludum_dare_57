using DG.Tweening;
using Player;
using UnityEngine;
using VContainer;

namespace ItemsSystem
{
    public class ItemObject : ACarryObject
    {
        private Rigidbody2D _playerRb;
        private Transform _startParent;

        [Inject]
        private void Construct(PlayerMovement playerMovement) => _playerRb = playerMovement.GetComponent<Rigidbody2D>();

        private void Start() => _startParent = transform.parent;

        public override void ResetObject()
        {
            DOTween.Kill(gameObject);
            transform.parent = _startParent;
            rb.linearVelocity = _playerRb.linearVelocity;

            EnablePhysics();
        }
    }
}