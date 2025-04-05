using InputSystem;
using InputSystem.Data;
using Player.Data;
using UnityEngine;
using VContainer;

namespace Player
{
    public class PlayerMovement : MonoBehaviour, IInputValueCallback<Vector2>, IInputCallback
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform checkGroundPoint;
        [SerializeField] private Vector2 groundCheckSize = Vector2.one;
        
        private bool _isGrounded;
        private PlayerSettingsSO _settings;
        private Vector2 _moveInput;
        private float _additionalFallSpeed;

        [Inject]
        private void Construct(PlayerInput input, PlayerSettingsSO settings)
        {
            _settings = settings;
            
            input.RegisterCallback(InputType.MOVE, this);
            input.RegisterNonValueCallback(InputType.JUMP, this);
        }
        
        private void Update()
        {
            CheckGround();

            var velocity = rb.linearVelocity;
            velocity.y -= _additionalFallSpeed;
            
            if (!_isGrounded)
            {
                _additionalFallSpeed = Mathf.Clamp(_additionalFallSpeed + _settings.FallIncreaseSpeed * Time.deltaTime, 0, _settings.MaxFallSpeed);
                rb.linearVelocity = velocity;
            }
            else
                _additionalFallSpeed = 0;

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

        // Move Event
        public void InputCallback(Vector2 value) => _moveInput = value;

        // Jump Event
        public void InputCallback() => Jump();

        private void FixedUpdate()
        {
            var velocity = rb.linearVelocity;
            velocity.x = _moveInput.x * _settings.PlayerSpeed;
            rb.linearVelocity = velocity;
        }

        private void Jump()
        {
            if(!_isGrounded)
                return;
            
            rb.AddForce(Vector2.up * _settings.JumpForce, ForceMode2D.Impulse);
        }
    }
}
