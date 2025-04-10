using System.Collections;
using Core;
using Player;
using Services;
using UnityEngine;
using VContainer;
using WeaponSystem;

namespace Enemies.Enemies
{
    public sealed class ShooterEnemy : AEnemy
    {
        private static readonly int _isGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int _isRunning = Animator.StringToHash("IsRunning");
        private static readonly int _attack = Animator.StringToHash("Attack");

        [SerializeField] private LayerMask shootCheckRayLayers;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float attackDistance;
        [SerializeField] private float saveJumpForce;
        [SerializeField] private float saveJumpCooldown;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float playerOffsetToJump = 0.35f;
        [SerializeField] private Animator animator;
        [SerializeField] private ParticleSystem fleshParticles;
        [SerializeField] private ShooterBullet bulletPrefab;
        [SerializeField] private Transform shootPoint;

        private PlayerMovement _playerMovement;
        private DictionaryObjectPool _pool;
        private bool _canJump = true;
        private bool _canAttack = true;
        private bool _isTryingAttack;
        private float _distanceToPlayer;
        private float _currentSpeed;
        private float _startSizeX;

        [Inject]
        private void Construct(PlayerMovement playerMovement, DictionaryObjectPool pool)
        {
            _playerMovement = playerMovement;
            _pool = pool;
        }

        protected override void Start()
        {
            base.Start();
            _currentSpeed = Random.Range(minSpeed, maxSpeed);
            _startSizeX = transform.localScale.x;
        }

        private void Update()
        {
            _distanceToPlayer = Vector2.Distance(transform.position, _playerMovement.transform.position);

            CheckGround();

            if (_distanceToPlayer <= attackDistance)
                Attack();
            else
                _isTryingAttack = false;

            if (_distanceToPlayer > attackDistance &&
                _playerMovement.transform.position.y - transform.position.y > playerOffsetToJump && !_isTryingAttack)
                JumpUp();


            animator.SetBool(_isGrounded, IsGrounded);

            if (!_isTryingAttack)
                return;

            var result = Physics2D.Raycast(shootPoint.position,
                (_playerMovement.transform.position - shootPoint.position).normalized, attackDistance,
                shootCheckRayLayers);

            if (!_canAttack || !LayerService.CheckLayersEquality(result.transform.gameObject.layer, playerLayer))
                return;

            animator.SetTrigger(_attack);
            _canAttack = false;
            StartCoroutine(AttackCooldownCoroutine());
        }

        private void FixedUpdate()
        {
            animator.SetBool(_isRunning, _canAttack && !_isTryingAttack);
            if (_canAttack && !_isTryingAttack)
                rb.linearVelocity =
                    new Vector2(
                        _playerMovement.transform.position.x > transform.position.x ? _currentSpeed : -_currentSpeed,
                        rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            RotateEnemy(_playerMovement.transform.position.x > transform.position.x);
        }

        protected override void Death()
        {
            fleshParticles.gameObject.SetActive(true);
            fleshParticles.Play();
            fleshParticles.transform.parent = null;
            ParticleLifeControl.RegisterParticleLifeWithTime(fleshParticles, fleshParticles.main.duration);
            base.Death();
        }

        private void JumpUp()
        {
            if (!_canJump || !IsGrounded)
                return;

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForceY(saveJumpForce, ForceMode2D.Impulse);

            StartCoroutine(JumpCooldownCoroutine());
        }

        private void Attack()
        {
            if (!_canAttack || !IsGrounded || _isTryingAttack)
                return;

            _isTryingAttack = true;
        }

        // Call from animation
        public void Shoot()
        {
            var projectile = _pool.TryPop(out ShooterBullet bullet)
                ? bullet!
                : GameInstaller.InstantiateInjectedObject(bulletPrefab.gameObject).GetComponent<ShooterBullet>();

            projectile.transform.position = shootPoint.position;

            var direction = _playerMovement.transform.position - shootPoint.position;
            direction.z = 0f;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
            projectile.InitPool(_pool);
            projectile.SetDamageAmount(GetDamageAmount());
            projectile.ActivateBullet();
            _canAttack = false;
            StartCoroutine(AttackCooldownCoroutine());
        }

        private void RotateEnemy(bool right)
        {
            transform.localScale = new Vector3(right ? _startSizeX : -_startSizeX, transform.localScale.y,
                transform.localScale.z);
        }

        private IEnumerator JumpCooldownCoroutine()
        {
            _canJump = false;

            yield return new WaitForSeconds(saveJumpCooldown);

            _canJump = true;
        }

        private IEnumerator AttackCooldownCoroutine()
        {
            _canAttack = false;

            yield return new WaitForSeconds(attackCooldown);

            _canAttack = true;
        }
    }
}