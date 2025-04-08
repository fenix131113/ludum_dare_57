using System;
using ItemsSystem.Objects;
using ItemsSystem.Player;
using TMPro;
using UnityEngine;
using VContainer;

namespace WeaponSystem.View
{
    public sealed class AmmoView : MonoBehaviour
    {
        [SerializeField] private TMP_Text ammoText;

        private ItemHolder _itemHolder;
        private AAmmoWeapon _currentWeapon;

        [Inject]
        private void Construct(ItemHolder itemHolder) => _itemHolder = itemHolder;

        private void Awake() => Bind();

        private void OnDestroy() => Expose();

        private void RedrawAmmo()
        {
            ammoText.gameObject.SetActive(_currentWeapon);
            
            if (_currentWeapon)
                ammoText.text = $"{_currentWeapon.CurrentAmmo}/{_currentWeapon.MaxAmmo}";
        }

        private void OnItemChanged(ACarryObject item)
        {
            if (item is not WeaponObject weaponObject ||
                weaponObject.GetWeaponBase() is not AAmmoWeapon ammoWeapon)
            {
                _currentWeapon = null;
                RedrawAmmo();
                return;
            }


            if (_currentWeapon)
                _currentWeapon.OnShoot -= RedrawAmmo;

            _currentWeapon = ammoWeapon;

            _currentWeapon.OnShoot += RedrawAmmo;
            RedrawAmmo();
        }

        private void Bind() => _itemHolder.OnCurrentItemChanged += OnItemChanged;

        private void Expose()
        {
            _itemHolder.OnCurrentItemChanged -= OnItemChanged;

            if (_currentWeapon)
                _currentWeapon.OnShoot -= RedrawAmmo;
        }
    }
}