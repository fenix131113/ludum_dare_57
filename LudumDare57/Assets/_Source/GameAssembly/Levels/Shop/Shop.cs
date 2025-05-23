﻿using System;
using System.Collections.Generic;
using System.Linq;
using Levels.Shop.View;
using Player;
using UnityEngine;
using Upgrades;
using VContainer;
using Random = UnityEngine.Random;

// ReSharper disable IdentifierTypo

namespace Levels.Shop
{
    public sealed class Shop : MonoBehaviour
    {
        [SerializeField] private ShopHUD shopHUD;
        [SerializeField] private Trader sofaTrader;
        [SerializeField] private Trader volosovTrader;
        [SerializeField] private Trader graykTrader;

        [SerializeField] private List<UpgradeSO> statsUpgrades;
        [SerializeField] private List<UpgradeSO> weaponUpgrades;
        [SerializeField] private List<UpgradeSO> abilitiesUpgrades;

        private Trader _currentTrader;
        private PlayerStats _playerStats;

        [Inject]
        private void Construct(PlayerStats playerStats) => _playerStats = playerStats;

        private void Start() => SelectTrader();

        private void SelectTrader()
        {
            var allowTraders = new List<Trader>();

            if (abilitiesUpgrades.Any(upgrade => !_playerStats.Upgrades.ContainsKey(upgrade) ||
                                                 _playerStats.Upgrades[upgrade] < upgrade.MaxLevel))
                allowTraders.Add(sofaTrader);

            if (weaponUpgrades.Any(upgrade => !_playerStats.Upgrades.ContainsKey(upgrade) ||
                                              _playerStats.Upgrades[upgrade] < upgrade.MaxLevel))
                allowTraders.Add(graykTrader);

            if (statsUpgrades.Any(upgrade => !_playerStats.Upgrades.ContainsKey(upgrade) ||
                                             _playerStats.Upgrades[upgrade] < upgrade.MaxLevel))
                allowTraders.Add(volosovTrader);

            if (allowTraders.Count == 0)
                return;

            var selectedTrader = allowTraders[Random.Range(0, allowTraders.Count)];

            _currentTrader = selectedTrader.UpgradeType switch
            {
                UpgradeType.ABILITIES => sofaTrader,
                UpgradeType.STATS => volosovTrader,
                UpgradeType.WEAPON => graykTrader,
                _ => throw new ArgumentException("Invalid trader index!")
            };

            shopHUD.BindTrader(_currentTrader);

            var selectedUpgrades = _currentTrader.UpgradeType switch
            {
                UpgradeType.ABILITIES => abilitiesUpgrades,
                UpgradeType.STATS => statsUpgrades,
                UpgradeType.WEAPON => weaponUpgrades,
                _ => throw new ArgumentOutOfRangeException()
            };

            var haveUpgrades = (from upgrade in _playerStats.Upgrades
                where upgrade.Value >= upgrade.Key.MaxLevel
                select upgrade.Key).ToList();

            var excepted = selectedUpgrades.Except(haveUpgrades).ToList();

            var sellUpgrades = new (UpgradeSO, int)[3];

            for (var i = 0; i < 3; i++)
            {
                if (excepted.Count == 0)
                    continue;

                var selected = excepted[Random.Range(0, excepted.Count)];
                sellUpgrades[i] = (selected, _playerStats.Upgrades.GetValueOrDefault(selected, 0) + 1);
                excepted.Remove(selected);
            }

            if (sellUpgrades.Length == 0)
                return;

            _currentTrader.gameObject.SetActive(true);
            shopHUD.ShowCards(sellUpgrades);
        }
    }
}