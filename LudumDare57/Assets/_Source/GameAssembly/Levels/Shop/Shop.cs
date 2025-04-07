using System;
using Levels.Shop.View;
using UnityEngine;
using Random = UnityEngine.Random;

// ReSharper disable IdentifierTypo

namespace Levels.Shop
{
    public class Shop : MonoBehaviour
    {
        [SerializeField] private ShopHUD shopHUD;
        [SerializeField] private Trader sofaTrader;
        [SerializeField] private Trader volosovTrader;
        [SerializeField] private Trader graykTrader;

        private Trader _currentTrader;
        
        private void Start()
        {
            SelectTrader();
        }

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
            _currentTrader.gameObject.SetActive(true);
        }
    }
}