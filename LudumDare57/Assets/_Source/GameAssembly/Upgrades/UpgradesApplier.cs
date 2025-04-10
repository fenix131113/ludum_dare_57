using System;
using ItemsSystem.Objects;
using Player;
using UnityEngine;
using VContainer;
using WeaponSystem;
using WeaponSystem.Weapons;

namespace Upgrades
{
    public class UpgradesApplier :  MonoBehaviour
    {
        [SerializeField] private AAmmoWeapon pistolWeapon;
        [SerializeField] private AAmmoWeapon autoRifleWeapon;
        [SerializeField] private Shotgun shotgunWeapon;
        [SerializeField] private WeaponSelector weaponSelector;
        
        private PlayerStats _playerStats;
        private PlayerHealth _playerHealth;
        private PlayerMovement _playerMovement;
        
        [Inject]
        private void Construct(PlayerStats playerStats, PlayerHealth playerHealth, PlayerMovement playerMovement)
        {
            _playerStats = playerStats;
            _playerHealth = playerHealth;
            _playerMovement = playerMovement;
        }

        private void Awake()
        {
            foreach (var upgrade in _playerStats.Upgrades)
            {
                switch (upgrade.Key.UpgradeItem)
                {
                    case UpgradeItem.DOUBLE_JUMP:
                        break;
                    case UpgradeItem.FAST_RELOAD:
                        shotgunWeapon.SetReloadTimeMultiplier(0.75f);
                        pistolWeapon.SetReloadTimeMultiplier(0.75f);
                        autoRifleWeapon.SetReloadTimeMultiplier(0.75f);
                        break;
                    case UpgradeItem.MORE_HEALTH:
                        _playerHealth.ChangeMaxHealth(_playerHealth.GetMaxHealth() + 2 * upgrade.Value);
                        break;
                    case UpgradeItem.MORE_STRONG:
                        break;
                    case UpgradeItem.MORE_SPEED:
                        _playerMovement.SetExtraMoveSpeed(upgrade.Key.FloatValue[upgrade.Value - 1]);
                        break;
                    case UpgradeItem.PISTOL_CLIP_INCREASE:
                        pistolWeapon.ChangeMaxAmmo(upgrade.Key.IntValue[upgrade.Value - 1]);
                        break;
                    case UpgradeItem.PISTOL_DAMAGE_INCREASE:
                        pistolWeapon.SetDamageAmount(upgrade.Key.IntValue[upgrade.Value - 1]);
                        break;
                    case UpgradeItem.BUY_SHOTGUN:
                        weaponSelector?.AddWeapon(shotgunWeapon.GetComponent<WeaponObject>());
                        break;
                    case UpgradeItem.SHOTGUN_CLIP_INCREASE:
                        shotgunWeapon.ChangeMaxAmmo(upgrade.Key.IntValue[upgrade.Value - 1]);
                        break;
                    case UpgradeItem.SHOTGUN_FRACTION_INCREASE:
                        shotgunWeapon.SetFractionAmount(upgrade.Key.IntValue[upgrade.Value - 1]);
                        break;
                    case UpgradeItem.AUTO_RIFLE_BUY:
                        weaponSelector?.AddWeapon(autoRifleWeapon.GetComponent<WeaponObject>());
                        break;
                    case UpgradeItem.AUTO_RIFLE_CLIP_INCREASE:
                        autoRifleWeapon.ChangeMaxAmmo(upgrade.Key.IntValue[upgrade.Value - 1]);
                        break;
                    case UpgradeItem.AUTO_RIFLE_DAMAGE_INCREASE:
                        autoRifleWeapon.SetDamageAmount(upgrade.Key.IntValue[upgrade.Value - 1]);
                        break;
                    case UpgradeItem.PICKAXE_ATTACK_RANGE_INCREASE:
                        break;
                    case UpgradeItem.PICKAXE_TAKE_RANGE_INCREASE:
                        break;
                    case UpgradeItem.DASH:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}