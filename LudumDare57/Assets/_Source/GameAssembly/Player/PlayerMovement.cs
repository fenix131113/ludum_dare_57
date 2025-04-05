using System.Collections;
using Environment;
using InputSystem;
using Player.Data;
using Services;
using UnityEngine;
using VContainer;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private LayerMask platformLayer;
        [SerializeField] private Transform checkGroundPoint;
        [SerializeField] private Vector2 groundCheckSize = Vector2.one;
        [SerializeField] private float checkPlatformDistance = 1f;
        [SerializeField] private float jumpCooldown = .5f;

        private bool _isGrounded;
        private PlayerSettingsSO _settings;
        private PlayerInput _input;
        private Vector2 _moveInput;
        private float _additionalFallSpeed;
        private bool _canJump = true;

        [Inject]
        private void Construct(PlayerInput input, PlayerSettingsSO settings)
        {
            _input = input;
            _settings = settings;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Update()
        {
            CheckGround();

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
            var velocity = rb.linearVelocity;
            velocity.x = _moveInput.x * _settings.PlayerSpeed;
            rb.linearVelocity = velocity;
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

        // Move Event
        public void OnMove(Vector2 value) => _moveInput = value;

        // Jump Event
        public void OnJump() => Jump();

        private void Jump()
        {
            if (!_isGrounded || !_canJump)
                return;

            StartCoroutine(JumpCooldown());
            rb.AddForce(Vector2.up * _settings.JumpForce, ForceMode2D.Impulse);
        }

        private void Bind()
        {
            _input.OnMove += OnMove;
            _input.OnJump += OnJump;
            _input.OnFall += PlatformFall;
        }

        private void Expose()
        {
            _input.OnMove -= OnMove;
            _input.OnJump -= OnJump;
            _input.OnFall -= PlatformFall;
        }

        private IEnumerator JumpCooldown()
        {
            _canJump = false;
            
            yield return new WaitForSeconds(jumpCooldown);

            _canJump = true;
        }
    }
}