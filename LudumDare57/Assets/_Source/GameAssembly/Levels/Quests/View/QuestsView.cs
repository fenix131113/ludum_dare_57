using TMPro;
using UnityEngine;
using VContainer;

namespace Levels.Quests.View
{
    public class QuestsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text questNameText;
        [SerializeField] private TMP_Text questDescriptionText;
        
        private QuestsManager _questsManager;
        
        [Inject]
        private void Construct(QuestsManager questsManager) => _questsManager = questsManager;

        private void Start()
        {
            Bind();
            Redraw();
        }

        private void OnDestroy() => Expose();

        private void Redraw()
        {
            questNameText.text = _questsManager.CurrentQuest.QuestName;
            questDescriptionText.text = string.Format(_questsManager.CurrentQuest.Description, _questsManager.CurrentValue, _questsManager.NeedValue);
        }

        private void Bind() => _questsManager.OnQuestValueChange += Redraw;

        private void Expose() => _questsManager.OnQuestValueChange -= Redraw;
    }
}