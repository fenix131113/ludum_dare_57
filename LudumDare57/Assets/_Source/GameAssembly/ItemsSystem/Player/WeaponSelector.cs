using System.Collections.Generic;
using InputSystem;
using UnityEngine;
using VContainer;
using WeaponSystem;

namespace ItemsSystem.Player
{
    public class WeaponSelector : MonoBehaviour
    {
        [SerializeField] private List<WeaponObject> weapons;

        private int _selectedWeaponIndex;
        private PlayerInput _input;
        private ItemHolder _itemHolder;

        [Inject]
        private void Construct(PlayerInput input, ItemHolder itemHolder)
        {
            _input = input;
            _itemHolder = itemHolder;
        }

        private void Start()
        {
            Bind();
            TakeCurrentWeapon();
        }

        private void OnDestroy() => Expose();

        private void OnScroll(float scrollValue)
        {
            switch (scrollValue)
            {
                case > 0:
                    ChangeWeaponIndex(true);
                    break;
                case < 0:
                    ChangeWeaponIndex(false);
                    break;
            }

            TakeCurrentWeapon();
        }

        private void TakeCurrentWeapon()
        {
            if (_itemHolder.CurrentObject && _itemHolder.CurrentObject == weapons[_selectedWeaponIndex])
                return;

            _itemHolder.TakeItem(weapons[_selectedWeaponIndex]);
        }

        private void ChangeWeaponIndex(bool plus)
        {
            if (plus)
            {
                if (_selectedWeaponIndex + 1 >= weapons.Count)
                    _selectedWeaponIndex = 0;
                else
                    _selectedWeaponIndex++;
            }
            else
            {
                if (_selectedWeaponIndex - 1 < 0)
                    _selectedWeaponIndex = weapons.Count - 1;
                else
                    _selectedWeaponIndex--;
            }
        }

        public void AddWeapon(WeaponObject weapon)
        {
            if (weapons.Contains(weapon))
                return;

            weapons.Add(weapon);
        }

        private void Bind() => _input.OnScroll += OnScroll;

        private void Expose() => _input.OnScroll -= OnScroll;
    }
}