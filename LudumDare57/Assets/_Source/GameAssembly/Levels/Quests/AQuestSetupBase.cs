using UnityEngine;

namespace Levels.Quests
{
    public abstract class AQuestSetupBase : MonoBehaviour
    {
        protected QuestsManager QuestsManager;

        public virtual void Setup(QuestsManager questsManager) => QuestsManager = questsManager;
    }
}