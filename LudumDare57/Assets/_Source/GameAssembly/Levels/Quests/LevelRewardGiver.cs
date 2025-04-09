using Interactable;
using UnityEngine;
using VContainer;

namespace Levels.Quests
{
    public class LevelRewardGiver : MonoBehaviour
    {
        [SerializeField] private Lever lever;

        private QuestsManager _questsManager;

        [Inject]
        private void Construct(QuestsManager questsManager) => _questsManager = questsManager;

        private void Start() => Bind();

        private void GiveReward() => _questsManager.GiveReward();

        private void Bind() => lever.OnLeverPressed += GiveReward; // Expose in lever
    }
}