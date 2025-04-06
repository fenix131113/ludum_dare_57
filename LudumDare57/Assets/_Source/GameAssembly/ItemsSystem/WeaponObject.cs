using UnityEngine;
using WeaponSystem;

namespace ItemsSystem
{
    [RequireComponent(typeof(AWeaponBase))]
    public class WeaponObject : ACarryObject
    {
        private AWeaponBase _weapon;

        private void Start() => _weapon = GetComponent<AWeaponBase>();

        public override void ResetObject()
        {
            if (_weapon is AAmmoWeapon ammoWeapon)
                ammoWeapon.CancelReload();

            gameObject.SetActive(false);
        }
    }
}