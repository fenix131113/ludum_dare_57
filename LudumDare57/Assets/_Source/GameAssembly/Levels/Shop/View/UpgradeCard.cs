using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;
using VContainer;

namespace Levels.Shop.View
{
    public sealed class UpgradeCard : MonoBehaviour
    {
        [SerializeField] private Button cardButton;
        [SerializeField] private TMP_Text costLabel;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text descriptionLabel;
        [SerializeField] private Image upgradeImage;

        private (UpgradeSO, int) _upgrade;

        private PlayerStats _playerStats;

        [Inject]
        private void Construct(PlayerStats playerStats) => _playerStats = playerStats;

        public void ActivateCard((UpgradeSO, int) upgrade)
        {
            _upgrade = upgrade;
            gameObject.SetActive(true);
            upgradeImage.sprite = upgrade.Item1.Icon;
            costLabel.text = _upgrade.Item1.Cost[upgrade.Item2 - 1].ToString() + "$";
            descriptionLabel.text = _upgrade.Item1.Description;
            nameLabel.text = _upgrade.Item1.UpgradeName;
        }

        private void OnCardClicked()
        {
            if (_playerStats.Coins < _upgrade.Item1.Cost[_upgrade.Item2 - 1])
                return;

            if (!_playerStats.TryRemoveCoins(_upgrade.Item1.Cost[_upgrade.Item2 - 1]))
                return;

            if (!_playerStats.Upgrades.TryAdd(_upgrade.Item1, _upgrade.Item2))
                _playerStats.Upgrades[_upgrade.Item1]++;

            gameObject.SetActive(false);
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind() => cardButton.onClick.AddListener(OnCardClicked);

        private void Expose() => cardButton.onClick.RemoveAllListeners();
    }
}