using System.Collections.Generic;
using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "UpgradeSO", menuName = "Configs/Upgrade")]
    public class UpgradeSO : ScriptableObject
    {
        [field: SerializeField] public UpgradeItem UpgradeItem { get; protected set; }
        [field: SerializeField] public string UpgradeName { get; protected set; }
        [field: SerializeField] public string Description { get; protected set; }
        [field: SerializeField] public int MaxLevel { get; protected set; }
        [field: SerializeField] public Sprite Icon { get; protected set; }
        [field: SerializeField] public List<int> Cost { get; protected set; }
        [field: SerializeField] public UpgradeType UpgradeType { get; protected set; }
        [field: SerializeField] public List<float> FloatValue { get; protected set; }
        [field: SerializeField] public List<int> IntValue { get; protected set; }
    }
}