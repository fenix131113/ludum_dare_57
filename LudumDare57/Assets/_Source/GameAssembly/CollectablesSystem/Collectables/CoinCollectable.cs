using Player;
using UnityEngine;
using VContainer;

namespace CollectablesSystem.Collectables
{
    public sealed class CoinCollectable : CollectableObject
    {
        [SerializeField] private int coinsAdd = 1;
        
        private PlayerStats _stats;
        
        [Inject]
        private void Construct(PlayerStats stats)
        {
            _stats = stats;
        }
        
        protected override void Collect()
        {
            _stats.AddCoins(coinsAdd);
            gameObject.SetActive(false);
        }
    }
}