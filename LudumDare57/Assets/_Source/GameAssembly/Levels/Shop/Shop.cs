using System;
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
            var traderIndex = Random.Range(0, 3);

            _currentTrader = traderIndex switch
            {
                0 => sofaTrader,
                1 => volosovTrader,
                2 => graykTrader,
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

            var excepted = new List<UpgradeSO>();
            foreach (var upgrade in selectedUpgrades)
            {
                if (_playerStats.Upgrades.ContainsKey(upgrade) && _playerStats.Upgrades[upgrade] >= upgrade.MaxLevel)
                    return;

                excepted.Add(upgrade);
            }

            _currentTrader.gameObject.SetActive(true);

            var sellUpgrades = new UpgradeSO[3];

            for (var i = 0; i < 3; i++)
            {
                if (excepted.Count == 0)
                    continue;

                sellUpgrades[i] = excepted[Random.Range(0, excepted.Count)];
                excepted.Remove(sellUpgrades[i]);
            }
            
            shopHUD.ShowCards(sellUpgrades);
        }
    }
}