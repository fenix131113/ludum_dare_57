using CollectablesSystem;
using UnityEngine;

namespace WeaponSystem.Weapons.Pickaxe
{
    public sealed class PickaxePickup : CollectableObject
    {
        [SerializeField] private ThrownPickaxe thrownPickaxe;
        
        protected override void Collect()
        {
            thrownPickaxe.ReturnPickaxe();
            gameObject.SetActive(false);
        }
    }
}