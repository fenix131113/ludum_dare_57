namespace ItemsSystem
{
    public class WeaponObject : ACarryObject
    {
        public override void ResetObject()
        {
            gameObject.SetActive(false);
        }
    }
}