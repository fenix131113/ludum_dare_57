using Core;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;
using VContainer;

namespace Levels.Shop.View
{
    public sealed class ShopHUD : MonoBehaviour
    {
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private Button closeButton;
        [SerializeField] private UpgradeCard[] upgradeCards;

        private Trader _trader;
        private GameState _gameState;

        [Inject]
        private void Construct(GameState gameState) => _gameState = gameState;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        public void OpenShopHUD()
        {
            shopPanel.SetActive(true);
            Cursor.visible = true;
            _gameState.SetPlayerMovementPaused(true);
        }

        public void CloseShopHUD()
        {
            shopPanel.SetActive(false);
            Cursor.visible = false;
            _gameState.SetPlayerMovementPaused(false);
        }

        private void OnInteractWithTrader()
        {
            OpenShopHUD();
        }

        public void ShowCards((UpgradeSO, int)[] upgrades)
        {
            for (var i = 0; i < upgradeCards.Length; i++)
            {
                if (upgrades[i].Item1)
                    upgradeCards[i]?.ActivateCard(upgrades[i]);
            }
        }

        public void BindTrader(Trader trader)
        {
            _trader = trader;
            _trader.OnInteract += OnInteractWithTrader;
        }

        private void Bind()
        {
            closeButton.onClick.AddListener(CloseShopHUD);
        }

        private void Expose()
        {
            _trader.OnInteract -= OnInteractWithTrader;
            closeButton.onClick.RemoveAllListeners();
        }
    }
}