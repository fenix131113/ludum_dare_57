using System.Collections;
using Player;
using UnityEngine;
using VContainer;

namespace Enemies.Enemies
{
    public sealed class RunnerEnemy : AEnemy
    {
        [SerializeField] private float minSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float attackDistance;
        [SerializeField] private float nearDistance;
        [SerializeField] private float saveJumpForce;
        [SerializeField] private float saveJumpCooldown;
        [SerializeField] private float attackHorizontalForce;
        [SerializeField] private float attackVerticalForce;
        [SerializeField] private float attackCooldown;
        [SerializeField] private Animator animator; // TODO: Make Jump Animation

        private PlayerMovement _playerMovement;
        private bool _firstlyNearPlayer;
        private bool _canJump = true;
        private bool _canAttack = true;
        private float _distanceToPlayer;
        private float _currentSpeed;

        [Inject]
        private void Construct(PlayerMovement playerMovement) => _playerMovement = playerMovement;

        protected override void Start()
        {
            base.Start();
            _currentSpeed = Random.Range(minSpeed, maxSpeed);
        }

        private void Update()
        {
            _distanceToPlayer = Vector2.Distance(transform.position, _playerMovement.transform.position);

            switch (_firstlyNearPlayer)
            {
                case false when _distanceToPlayer <= nearDistance:
                    _firstlyNearPlayer = true;
                    break;
                case true when _distanceToPlayer > nearDistance:
                    CheckGround();
                    JumpUp();
                    break;
                case true when _distanceToPlayer <= attackDistance:
                    CheckGround();
                    Attack();
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (_canAttack)
                rb.linearVelocity =
                    new Vector2(
                        _playerMovement.transform.position.x > transform.position.x ? _currentSpeed : -_currentSpeed,
                        rb.linearVelocity.y);

            RotateEnemy(_playerMovement.transform.position.x > transform.position.x);
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
            if (!_canAttack || !IsGrounded)
                return;

            _firstlyNearPlayer = false;

            var xForce = _playerMovement.transform.position.x > transform.position.x
                ? attackHorizontalForce
                : -attackHorizontalForce;

            rb.linearVelocity = new Vector2(0f, 0f);
            rb.AddForce(new Vector2(xForce, attackVerticalForce), ForceMode2D.Impulse);
            StartCoroutine(AttackCooldownCoroutine());
        }

        private void RotateEnemy(bool right)
        {
            rootEnemyRenderer.flipX = !right;
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