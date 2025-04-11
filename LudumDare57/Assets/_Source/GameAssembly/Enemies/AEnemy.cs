using System;
using CollectablesSystem;
using Core;
using DG.Tweening;
using HealthSystem;
using Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class AEnemy : MonoBehaviour, IHealth, IDamageApplier
    {
        [SerializeField] protected LayerMask groundLayers;
        [SerializeField] protected Transform checkGroundPoint;
        [SerializeField] protected Vector2 groundCheckSize;
        [SerializeField] protected SpriteRenderer rootEnemyRenderer;
        [SerializeField] protected Rigidbody2D rb;
        [SerializeField] protected float damageAnimDuration;
        [SerializeField] protected CollectableObject dropItem;
        [SerializeField][Range(0f, 1f)] protected float dropItemChance;
        [SerializeField] private AudioClip[] deathSounds;

        [SerializeField] private int maxHealth;
        [SerializeField] private int damage;

        private Tween _damageTween;
        protected bool IsGrounded;

        private int _health;

        public event Action<AEnemy> OnDeath;

        protected virtual void Start() => _health = maxHealth;

        private void OnDestroy() => Expose();

        protected virtual void Death()
        {
            if (dropItem && Random.Range(0f, 1f) <= dropItemChance)
            {
                var spawned = GameInstaller.InstantiateInjectedObject(dropItem.gameObject);
                spawned.transform.position = transform.position;
            }
            
            if(deathSounds.Length > 0)
                SoundPlayService.Instance.PlaySound(deathSounds[Random.Range(0, deathSounds.Length)]);
            
            OnDeath?.Invoke(this);
            Expose();
            Destroy(gameObject); //TODO: Make Object Pool
        }

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
            _damageTween?.Kill();
            _damageTween = rootEnemyRenderer.DOColor(Color.red, damageAnimDuration / 2);
            _damageTween.onComplete += () =>
                _damageTween = rootEnemyRenderer.DOColor(Color.white, damageAnimDuration / 2);

            _health = Mathf.Clamp(_health - amount, 0, maxHealth);

            if (_health == 0)
                Death();
        }

        public void SetDamageAmount(int newDamage) => damage = Mathf.Clamp(newDamage, 0, maxHealth);

        public int GetDamageAmount() => damage;

        private void Expose() => OnDeath = null;
    }
}