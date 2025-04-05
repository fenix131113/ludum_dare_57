namespace ItemsSystem
{
    public class WeaponObject : CarryObject
    {
        public override void ResetObject()
        {
            gameObject.SetActive(false);
        }
    }
}