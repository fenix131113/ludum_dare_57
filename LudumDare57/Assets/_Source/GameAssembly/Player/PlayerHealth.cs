using System;
using HealthSystem;
using Player.Data;
using UnityEngine;
using VContainer;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        private int _health;
        private int _maxHealth;

        public event Action OnHealthChanged;

        [Inject]
        public PlayerHealth(PlayerSettingsSO playerSettings)
        {
            _maxHealth = playerSettings.MaxHealth;
            _health = _maxHealth;
        }

        public int GetHealth() => _maxHealth;

        public int GetMaxHealth() => _maxHealth;

        public bool CanGetDamage() => true;

        public void ChangeMaxHealth(int amount)
        {
            _maxHealth = Mathf.Abs(amount);
            OnHealthChanged?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            _health = Mathf.Clamp(_health - Mathf.Abs(damage), 0, _maxHealth);
            OnHealthChanged?.Invoke();
        }

        public void Heal(int amount)
        {
            _health = Mathf.Clamp(_health + Mathf.Abs(amount), 0, _maxHealth);
            OnHealthChanged?.Invoke();
        }
    }
}