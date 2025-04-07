using System.Linq;
using Core;
using Environment;
using UnityEngine;

namespace Levels.Quests.ConcreteQuests
{
    public class LoaderQuest : AQuestSetupBase
    {
        [SerializeField] private CallbackTrigger callbackTrigger;
        [SerializeField] private GameObject questBoxPrefab;
        
        public override void Setup(QuestsManager questsManager)
        {
            base.Setup(questsManager);
            callbackTrigger.gameObject.SetActive(true);
            
            SpawnSpecialItems();
            
            Bind();
        }

        private void OnDestroy() => Expose();

        private void SpawnSpecialItems()
        {
            var allowedToSpawnPoints = QuestsManager.GetSpecialItemsPoints().ToList();
            
            for (var i = 0; i < QuestsManager.NeedValue; i++)
            {
                var spawned = GameInstaller.InstantiateInjectedObject(questBoxPrefab);
                
                var spawnPoint = allowedToSpawnPoints[Random.Range(0, allowedToSpawnPoints.Count)];
                allowedToSpawnPoints.Remove(spawnPoint);
                
                spawned.transform.position = spawnPoint.transform.position;
            }
        }
        
        private void OnObjDroppedToLift(GameObject obj)
        {
            if(!obj.GetComponent<QuestItem>())
                return;

            QuestsManager.SetQuestProgressValue(QuestsManager.CurrentValue + 1);
        }

        private void OnObjTakeFromLift(GameObject obj)
        {
            if(!obj.GetComponent<QuestItem>())
                return;

            QuestsManager.SetQuestProgressValue(QuestsManager.CurrentValue - 1);
        }
        
        private void Bind()
        {
            callbackTrigger.OnTargetEntered += OnObjDroppedToLift;
            callbackTrigger.OnTargetExit += OnObjTakeFromLift;
        }

        private void Expose()
        {
            callbackTrigger.OnTargetEntered -= OnObjDroppedToLift;
            callbackTrigger.OnTargetExit -= OnObjTakeFromLift;
        }
    }
}