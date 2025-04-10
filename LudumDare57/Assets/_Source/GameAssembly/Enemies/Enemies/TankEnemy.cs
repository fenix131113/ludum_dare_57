using System.Collections;
using HealthSystem;
using Player;
using Services;
using UnityEngine;
using VContainer;

namespace Enemies.Enemies
{
    public class TankEnemy : AEnemy
    {
        private static readonly int _isGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int _attacking = Animator.StringToHash("IsAttacking");
        private static readonly int _jump = Animator.StringToHash("Jump");
        private static readonly int _hit = Animator.StringToHash("Hit");

        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float attackDistance;
        [SerializeField] private float nearDistance;
        [SerializeField] private float saveJumpForce;
        [SerializeField] private float saveJumpCooldown;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float playerOffsetToJump = 0.35f;
        [SerializeField] private Animator animator; // TODO: Make Jump Animation
        [SerializeField] private HistoryOnceDamageTrigger attackTrigger;
        [SerializeField] private ParticleSystem fleshParticles;

        private PlayerMovement _playerMovement;
        private bool _canJump = true;
        private bool _canAttack = true;
        private bool _isAttacking;
        private float _distanceToPlayer;
        private float _currentSpeed;
        private float _startSizeX;

        [Inject]
        private void Construct(PlayerMovement playerMovement) => _playerMovement = playerMovement;

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
            else if (_playerMovement.transform.position.y - transform.position.y > playerOffsetToJump && !_isAttacking)
                JumpUp();


            animator.SetBool(_isGrounded, IsGrounded);
        }

        private void FixedUpdate()
        {
            if (_canAttack && !_isAttacking)
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

            animator.SetTrigger(_jump);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForceY(saveJumpForce, ForceMode2D.Impulse);

            StartCoroutine(JumpCooldownCoroutine());
        }

        private void Attack()
        {
            if (!_canAttack || !IsGrounded || _isAttacking)
                return;
            
            animator.SetBool(_attacking, _isAttacking);
            animator.SetTrigger(_hit);
            StartCoroutine(AttackCooldownCoroutine());
        }

        // Call from animation
        public void ActivateDamageZone()
        {
            attackTrigger.gameObject.SetActive(true);
        }

        // Call from animation
        public void DeactivateDamageZone()
        {
            attackTrigger.gameObject.SetActive(false);
            attackTrigger.ResetHistory();
        }

        private void RotateEnemy(bool right)
        {
            transform.localScale = new Vector3(right ? _startSizeX : -_startSizeX, transform.localScale.y, transform.localScale.z);;
        }

        private IEnumerator JumpCooldownCoroutine()
        {
            _canJump = false;

            yield return new WaitForSeconds(saveJumpCooldown);

            _canJump = true;
        }

        private IEnumerator AttackCooldownCoroutine()
        {
            _isAttacking = true;
            _canAttack = false;
            animator.SetBool(_attacking, _isAttacking);
            yield return new WaitForSeconds(attackCooldown);

            _isAttacking = false;
            _canAttack = true;
            animator.SetBool(_attacking, _isAttacking);
        }
    }
}