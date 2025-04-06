using UnityEngine;

namespace ItemsSystem.Objects
{
    public abstract class ACarryObject : MonoBehaviour
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public bool IsPowerDepend { get; private set; }
        [field: SerializeField][field: Range(0, 1f)] public float WeightSpeedDownPercent { get; private set; } = 1f;

        [SerializeField] protected Rigidbody2D rb;

        public abstract void ResetObject();

        public void DisablePhysics()
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = false;
        }

        public void EnablePhysics()
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.simulated = true;
        }
    }
}