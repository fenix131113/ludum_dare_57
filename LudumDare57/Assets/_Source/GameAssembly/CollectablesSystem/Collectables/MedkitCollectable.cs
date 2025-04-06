using Player;
using UnityEngine;
using VContainer;

namespace CollectablesSystem.Collectables
{
    public sealed class MedkitCollectable : CollectableObject
    {
        [SerializeField] private int addHealth;

        private PlayerHealth _playerHealth;
        
        [Inject]
        private void Construct(PlayerHealth playerHealth) => _playerHealth = playerHealth;

        protected override void Collect()
        {
            gameObject.SetActive(false);
            _playerHealth.Heal(addHealth);
        }
    }
}