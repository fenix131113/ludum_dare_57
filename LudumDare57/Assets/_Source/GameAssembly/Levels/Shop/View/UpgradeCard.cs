using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Upgrades;

namespace Levels.Shop.View
{
    public sealed class UpgradeCard : MonoBehaviour
    {
        [SerializeField] private Button cardButton;
        [SerializeField] private TMP_Text costLabel;
        [SerializeField] private TMP_Text nameLabel;
        [SerializeField] private TMP_Text descriptionLabel;

        private UpgradeSO _upgrade;

        public void ActivateCard(UpgradeSO upgrade)
        {
            _upgrade = upgrade;
            gameObject.SetActive(true);
            costLabel.text = _upgrade.Cost.ToString();
            descriptionLabel.text = _upgrade.Description;
            nameLabel.text = _upgrade.UpgradeName;
        }

        private void OnCardClicked()
        {
            
        }
        
        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void Bind() => cardButton.onClick.AddListener(OnCardClicked);

        private void Expose() => cardButton.onClick.RemoveAllListeners();
    }
}