using VContainer;

namespace CollectablesSystem.Collectables
{
    public sealed class MedkitCollectable : CollectableObject
    {
        [Inject]
        private void Construct()
        {
            //TODO: Add medkit logic
        }
        
        protected override void Collect()
        {
            gameObject.SetActive(false);
        }
    }
}