using System.Collections;
using HealthSystem;
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
        private float _startSizeY;

        private void Awake()
        {
            _currentRotateSpeed = rotateSpeed;
            _startParent = transform.parent;
            _startSizeY = transform.localScale.y;
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

            // Rotate pickaxe after parent assign
            transform.localScale = new Vector3(transform.localScale.x,
                _startSizeY, transform.localScale.z);

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