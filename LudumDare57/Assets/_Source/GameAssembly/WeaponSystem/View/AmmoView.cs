using ItemsSystem.Objects;
using ItemsSystem.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace WeaponSystem.View
{
    public sealed class AmmoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text ammoText;
        [SerializeField] private Image reloadProgressFill;

        private ItemHolder _itemHolder;
        private AAmmoWeapon _currentWeapon;

        [Inject]
        private void Construct(ItemHolder itemHolder) => _itemHolder = itemHolder;

        private void Awake() => Bind();

        private void OnDestroy() => Expose();

        private void Update()
        {
            if(!_currentWeapon || !_currentWeapon.IsReloading)
                return;
            
            reloadProgressFill.gameObject.SetActive(true);
            reloadProgressFill.fillAmount = _currentWeapon.ReloadProgress;
        }

        private void RedrawAmmo()
        {
            ammoText.gameObject.SetActive(_currentWeapon);
            reloadProgressFill.gameObject.SetActive(_currentWeapon);
            reloadProgressFill.fillAmount = 0f;
            
            if (_currentWeapon)
                ammoText.text = $"{_currentWeapon.CurrentAmmo}/{_currentWeapon.MaxAmmo}";
        }

        private void OnItemChanged(ACarryObject item)
        {
            if (item is not WeaponObject { GetWeaponBase: AAmmoWeapon ammoWeapon })
            {
                _currentWeapon = null;
                RedrawAmmo();
                return;
            }


            if (_currentWeapon)
                _currentWeapon.OnAmmoChanged -= RedrawAmmo;

            _currentWeapon = ammoWeapon;

            _currentWeapon.OnAmmoChanged += RedrawAmmo;
            RedrawAmmo();
        }

        private void Bind() => _itemHolder.OnCurrentItemChanged += OnItemChanged;

        private void Expose()
        {
            _itemHolder.OnCurrentItemChanged -= OnItemChanged;

            if (_currentWeapon)
                _currentWeapon.OnAmmoChanged -= RedrawAmmo;
        }
    }
}