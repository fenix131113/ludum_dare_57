using UnityEngine;
using WeaponSystem;

namespace ItemsSystem.Objects
{
    [RequireComponent(typeof(AWeaponBase))]
    public class WeaponObject : ACarryObject
    {
        private AWeaponBase _weapon;

        public AWeaponBase GetWeaponBase
        {
            get
            {
                if (!_weapon)
                    _weapon = GetComponent<AWeaponBase>();

                return _weapon;
            }
        }

        public override void ResetObject()
        {
            if (_weapon is AAmmoWeapon ammoWeapon)
                ammoWeapon.CancelReload();

            gameObject.SetActive(false);
        }
    }
}