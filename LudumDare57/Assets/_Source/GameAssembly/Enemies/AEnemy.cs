using HealthSystem;
using UnityEngine;

namespace Enemies
{
    public class AEnemy : MonoBehaviour, IHealth, IDamageApplier
    {
        [SerializeField] protected LayerMask groundLayers;
        [SerializeField] protected Transform checkGroundPoint;
        [SerializeField] protected Vector2 groundCheckSize;
        [SerializeField] protected SpriteRenderer rootEnemyRenderer;
        [SerializeField] protected Rigidbody2D rb;

        [SerializeField] private int maxHealth;
        [SerializeField] private int damage;

        protected bool IsGrounded;

        private int _health;

        protected virtual void Start() => _health = maxHealth;

        protected virtual void Death() => gameObject.SetActive(false);

        protected void CheckGround()
        {
            if (IsGrounded == false)
                IsGrounded = rb.IsTouchingLayers(groundLayers) &&
                             Physics2D.OverlapBox(checkGroundPoint.position, groundCheckSize, 0f, groundLayers);
            else
                IsGrounded = Physics2D.OverlapBox(checkGroundPoint.position, groundCheckSize, 0f, groundLayers);
        }

        public int GetHealth() => _health;

        public int GetMaxHealth() => maxHealth;

        public bool CanGetDamage() => true;

        public void TakeDamage(int amount)
        {
            _health = Mathf.Clamp(_health - amount, 0, maxHealth);

            if (_health == 0)
                Death();
        }

        public void SetDamageAmount(int newDamage) => damage = Mathf.Clamp(newDamage, 0, maxHealth);

        public int GetDamageAmount() => damage;
    }
}