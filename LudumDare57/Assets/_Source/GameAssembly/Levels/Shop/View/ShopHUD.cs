using UnityEngine;

namespace Levels.Shop.View
{
    public class ShopHUD : MonoBehaviour
    {
        private Trader _trader;

        private void OnDestroy() => Expose();

        private void OnInteractWithTrader()
        {
            
        }
        
        public void BindTrader(Trader trader)
        {
            _trader = trader;
            _trader.OnInteract += OnInteractWithTrader;
        }

        private void Expose() => _trader.OnInteract -= OnInteractWithTrader;
    }
}