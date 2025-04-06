using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "PlayerSettingsSO", menuName = "Configs/PlayerSettingsSO")]
    public sealed class PlayerSettingsSO : ScriptableObject
    {
        [field: SerializeField] public int MaxHealth { get; private set; }
        [field: SerializeField] public float PlayerSpeed { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float FallIncreaseSpeed { get; private set; }
        [field: SerializeField] public float MaxFallSpeed { get; private set; }
    }
}