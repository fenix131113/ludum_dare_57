using System;
using System.Collections;
using Core;
using HealthSystem;
using Player.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private float afterDamageImmuneTime = 0.15f;
        
        private int _health;
        private int _maxHealth;
        private bool _afterDamageImmune;
        private GameState _gameState;

        public event Action OnHealthChanged;

        [Inject]
        public void Construct(PlayerSettingsSO playerSettings, GameState gameState)
        {
            _maxHealth = playerSettings.MaxHealth;
            _health = _maxHealth;
            _gameState = gameState;
        }

        public int GetHealth() => _health;

        public int GetMaxHealth() => _maxHealth;

        public bool CanGetDamage() => !_gameState.PlayerDamagePaused && !_afterDamageImmune;

        public void ChangeMaxHealth(int amount)
        {
            _maxHealth = amount;
            _health = _maxHealth;
            OnHealthChanged?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            if(!CanGetDamage())
                return;

            StartCoroutine(AfterDamageImmuneCoroutine());
            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
            OnHealthChanged?.Invoke();

            //TODO: refactor this
            if (_health == 0)
                SceneManager.LoadScene(0);
        }

        public void Heal(int amount)
        {
            _health = Mathf.Clamp(_health + Mathf.Abs(amount), 0, _maxHealth);
            OnHealthChanged?.Invoke();
        }

        private IEnumerator AfterDamageImmuneCoroutine()
        {
            _afterDamageImmune = true;
            
            yield return new WaitForSeconds(afterDamageImmuneTime);
            
            _afterDamageImmune = false;
        }
    }
}