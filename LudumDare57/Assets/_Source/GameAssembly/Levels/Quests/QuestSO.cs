using UnityEngine;

namespace Levels.Quests
{
    [CreateAssetMenu(fileName = "QuestSO", menuName = "Configs/QuestSO")]
    public class QuestSO : ScriptableObject
    {
        [field: SerializeField] public string QuestName { get; set; }
        [field: SerializeField] public string Description { get; set; }
        [field: SerializeField] public int NeedValueMin { get; set; }
        [field: SerializeField] public int NeedValueMax { get; set; }
    }
}