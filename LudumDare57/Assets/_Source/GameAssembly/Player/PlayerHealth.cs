﻿using System;
using System.Collections;
using Core;
using DG.Tweening;
using HealthSystem;
using Player.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private SpriteRenderer playerRenderer;
        [SerializeField] private float afterDamageImmuneTime = 0.15f;
        [SerializeField] private float damageAnimDuration = 0.125f;
        
        private int _health;
        private int _maxHealth;
        private bool _afterDamageImmune;
        private GameState _gameState;
        private PlayerStats _playerStats;
        private Tween _damageTween;

        public event Action OnHealthChanged;

        [Inject]
        public void Construct(PlayerSettingsSO playerSettings, GameState gameState, PlayerStats playerStats)
        {
            _maxHealth = playerSettings.MaxHealth;
            _health = _maxHealth;
            _gameState = gameState;
            _playerStats = playerStats;
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

            _damageTween?.Kill();
            _damageTween = playerRenderer.DOColor(Color.red, damageAnimDuration / 2);
            _damageTween.onComplete += () =>
                _damageTween = playerRenderer.DOColor(Color.white, damageAnimDuration / 2);
            
            StartCoroutine(AfterDamageImmuneCoroutine());
            _health = Mathf.Clamp(_health - damage, 0, _maxHealth);
            OnHealthChanged?.Invoke();

            //TODO: refactor this
            if (_health != 0)
                return;
            
            _playerStats.ResetStats();
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