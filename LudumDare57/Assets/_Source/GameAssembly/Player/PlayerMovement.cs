using System.Collections;
using Core;
using Environment;
using InputSystem;
using ItemsSystem.Player;
using Player.Data;
using Services;
using UnityEngine;
using VContainer;

namespace Player
{
    public sealed class PlayerMovement : MonoBehaviour
    {
        private static readonly int _groundedKey = Animator.StringToHash("IsGrounded");
        private static readonly int _isRunningKey = Animator.StringToHash("IsRunning");
        private static readonly int _jumpKey = Animator.StringToHash("Jump");
        
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask platformLayer;
        [SerializeField] private Transform checkGroundPoint;
        [SerializeField] private Vector2 groundCheckSize = Vector2.one;
        [SerializeField] private float checkPlatformDistance = 1f;
        [SerializeField] private float jumpCooldown = .5f;
        [SerializeField] private Animator anim;

        private bool _isGrounded;
        private PlayerSettingsSO _settings;
        private PlayerInput _input;
        private Vector2 _moveInput;
        private float _additionalFallSpeed;
        private float _extraSpeed = 1f;
        private bool _canJump = true;
        private ItemHolder _itemHolder;
        private GameState _gameState;

        [Inject]
        private void Construct(PlayerInput input, PlayerSettingsSO settings, ItemHolder itemHolder, GameState gameState)
        {
            _input = input;
            _settings = settings;
            _gameState = gameState;
            _itemHolder = itemHolder;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Update()
        {
            CheckGround();
            anim.SetBool(_groundedKey, _isGrounded);

            var velocity = rb.linearVelocity;
            velocity.y -= _additionalFallSpeed;

            if (!_isGrounded)
            {
                _additionalFallSpeed = Mathf.Clamp(_additionalFallSpeed + _settings.FallIncreaseSpeed * Time.deltaTime,
                    0, _settings.MaxFallSpeed);
                rb.linearVelocity = velocity;
            }
            else
                _additionalFallSpeed = 0;

            rb.linearVelocity = velocity;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (_gameState.GameCycleBlocked)
                return;

            var velocity = rb.linearVelocity;
            velocity.x = _moveInput.x * _settings.PlayerSpeed * _extraSpeed;

            // Apply object weight
            if (_itemHolder.CurrentObject && _itemHolder.CurrentObject.IsPowerDepend)
                velocity.x *= _itemHolder.CurrentObject.WeightSpeedDownPercent;

            rb.linearVelocity = velocity;

            anim.SetBool(_isRunningKey, rb.linearVelocity.x != 0);
        }

        private void CheckGround()
        {
            if (_isGrounded == false)
                _isGrounded = rb.IsTouchingLayers(groundLayer) &&
                              Physics2D.OverlapBox(checkGroundPoint.position, groundCheckSize, 0f, groundLayer);
            else
                _isGrounded = Physics2D.OverlapBox(checkGroundPoint.position, groundCheckSize, 0f, groundLayer);
        }

        private void PlatformFall()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, checkPlatformDistance, groundLayer);

            if (hit && LayerService.CheckLayersEquality(hit.collider.gameObject.layer, platformLayer))
                hit.transform.GetComponent<Platform>().DeactivateCollision();
        }

        private void Jump()
        {
            if (!_isGrounded || !_canJump || _gameState.GameCycleBlocked)
                return;

            anim.SetTrigger(_jumpKey);
            StartCoroutine(JumpCooldown());
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * _settings.JumpForce, ForceMode2D.Impulse);
        }

        private void OnGameCycleChanged(bool value)
        {
            if (value)
                rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }

        // Move Event
        public void OnMove(Vector2 value) => _moveInput = value;

        // Jump Event
        public void OnJump() => Jump();

        public void SetExtraMoveSpeed(float value) => _extraSpeed = value;

        private void Bind()
        {
            _input.OnMove += OnMove;
            _input.OnJump += OnJump;
            _input.OnFall += PlatformFall;
            _gameState.OnGameCycleBlockedChanged += OnGameCycleChanged;
        }

        private void Expose()
        {
            _input.OnMove -= OnMove;
            _input.OnJump -= OnJump;
            _input.OnFall -= PlatformFall;
            _gameState.OnGameCycleBlockedChanged -= OnGameCycleChanged;
        }

        private IEnumerator JumpCooldown()
        {
            _canJump = false;

            yield return new WaitForSeconds(jumpCooldown);

            _canJump = true;
        }
    }
}