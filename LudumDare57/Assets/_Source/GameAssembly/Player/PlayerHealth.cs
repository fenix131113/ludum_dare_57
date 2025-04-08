using System;
using HealthSystem;
using Player.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        private int _health;
        private int _maxHealth;

        public event Action OnHealthChanged;

        [Inject]
        public void Construct(PlayerSettingsSO playerSettings)
        {
            _maxHealth = playerSettings.MaxHealth;
            _health = _maxHealth;
        }

        public int GetHealth() => _health;

        public int GetMaxHealth() => _maxHealth;

        public void ChangeMaxHealth(int amount)
        {
            _maxHealth = amount;
            OnHealthChanged?.Invoke();
        }

        public void TakeDamage(int damage)
        {
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
    }
}