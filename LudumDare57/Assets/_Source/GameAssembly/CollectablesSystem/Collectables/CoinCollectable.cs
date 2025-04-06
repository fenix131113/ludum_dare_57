using UnityEngine;
using VContainer;

namespace CollectablesSystem.Collectables
{
    public sealed class CoinCollectable : CollectableObject
    {
        [Inject]
        private void Construct()
        {
            //TODO: Add coin logic
        }
        
        protected override void Collect()
        {
            gameObject.SetActive(false);
        }
    }
}