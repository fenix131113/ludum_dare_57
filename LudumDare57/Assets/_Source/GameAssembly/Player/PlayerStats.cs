using System;
using System.Collections.Generic;
using UnityEngine;
using Upgrades;

namespace Player
{
    public sealed class PlayerStats : MonoBehaviour
    {
        public int Coins { get; private set; }
        public Dictionary<UpgradeSO, int> Upgrades { get; private set; } = new();
        
        public event Action OnCoinsChanged;

        private void Awake() => DontDestroyOnLoad(gameObject);

        public void AddCoins(int amount)
        {
            Coins = Mathf.Clamp(Coins + amount, 0, int.MaxValue);
            OnCoinsChanged?.Invoke();
        }

        public bool TryRemoveCoins(int amount)
        {
            if (Coins < amount)
                return false;

            Coins = Mathf.Clamp(Coins - amount, 0, int.MaxValue);
            OnCoinsChanged?.Invoke();
            return true;
        }

        public void ResetStats()
        {
            Coins = 0;
            OnCoinsChanged?.Invoke();
        }
    }
}