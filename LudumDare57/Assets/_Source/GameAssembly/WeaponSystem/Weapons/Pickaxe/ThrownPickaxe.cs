using System.Collections;
using HealthSystem;
using Player;
using UnityEngine;

namespace WeaponSystem.Weapons.Pickaxe
{
    public sealed class ThrownPickaxe : HistoryOnceDamageTrigger
    {
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float downRotateSpeed;
        [SerializeField] private float throwForce;
        [SerializeField] private float autoReturnTime;
        [SerializeField] private PickaxeStuckChecker stuckChecker;
        [SerializeField] private Collider2D damageCollider;

        public bool Thrown { get; private set; }
        public bool Stuck { get; private set; }

        private float _currentRotateSpeed;
        private Rigidbody2D _rb;
        private Transform _startParent;
        private Coroutine _pickaxeReturnCoroutine;
        private bool _throwOnRightRotate;

        private void Awake()
        {
            _currentRotateSpeed = rotateSpeed;
            _startParent = transform.parent;
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!Thrown || Stuck)
                return;

            transform.Rotate(0, 0, _currentRotateSpeed * Time.fixedDeltaTime);
            _currentRotateSpeed = Mathf.Clamp(_currentRotateSpeed + Time.fixedDeltaTime * downRotateSpeed,
                rotateSpeed,
                rotateSpeed / 2);
        }

        public void StuckPickaxe()
        {
            if (Stuck || !Thrown)
                return;

            Stuck = true;
            damageCollider.enabled = false;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.linearVelocity = Vector2.zero;
        }

        public void Throw()
        {
            if (!gameObject.activeInHierarchy || Thrown)
                return;
            
            var shootPointRotate = transform.parent.parent;
            _throwOnRightRotate = shootPointRotate.localScale.y > 0;
            
            stuckChecker.gameObject.SetActive(true);
            Thrown = true;
            _rb.bodyType = RigidbodyType2D.Dynamic;
            _rb.AddForce(transform.parent.right * throwForce, ForceMode2D.Impulse);
            transform.parent = null;
            damageCollider.enabled = true;

            _pickaxeReturnCoroutine = StartCoroutine(ReturnPickaxeCooldown());
        }

        public void ReturnPickaxe()
        {
            ResetHistory();

            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            Stuck = false;
            Thrown = false;
            _currentRotateSpeed = rotateSpeed;
            transform.parent = _startParent;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _rb.linearVelocity = Vector2.zero;
            transform.localPosition = Vector3.zero;
            transform.rotation = transform.parent.parent.rotation;
            damageCollider.enabled = false;
            
            var shootPointRotate = transform.parent.parent;

            var currentRightRotate = shootPointRotate.localScale.y > 0;
            
            // Rotate pickaxe after parent assign
            if (_throwOnRightRotate != currentRightRotate)
                transform.localScale = new Vector3(transform.localScale.x,
                    -transform.localScale.y, transform.localScale.z);

            if (_pickaxeReturnCoroutine != null)
                StopCoroutine(_pickaxeReturnCoroutine);
        }

        private IEnumerator ReturnPickaxeCooldown()
        {
            yield return new WaitForSeconds(autoReturnTime);

            ReturnPickaxe();
        }
    }
}