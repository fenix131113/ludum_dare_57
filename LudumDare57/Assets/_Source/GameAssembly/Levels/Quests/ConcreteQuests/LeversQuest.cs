using System.Collections.Generic;
using System.Linq;
using Core;
using Interactable;
using UnityEngine;

namespace Levels.Quests.ConcreteQuests
{
    public class LeversQuest : AQuestSetupBase
    {
        [SerializeField] private GameObject leverPrefab;
        [SerializeField] private int rewardPerLever;
        
        public override int GetReward() => rewardPerLever * QuestsManager.NeedValue;

        public override void Setup(QuestsManager questsManager)
        {
            base.Setup(questsManager);
            
            SpawnSpecialItems();
        }

        private void IncreaseProgress()
        {
            QuestsManager.SetQuestProgressValue(QuestsManager.CurrentValue + 1);
        }

        private void SpawnSpecialItems()
        {
            var allowedToSpawnPoints = QuestsManager.GetSpecialItemsPoints().ToList();
            
            for (var i = 0; i < QuestsManager.NeedValue; i++)
            {
                var spawned = GameInstaller.InstantiateInjectedObject(leverPrefab);
                
                var spawnPoint = allowedToSpawnPoints[Random.Range(0, allowedToSpawnPoints.Count)];
                allowedToSpawnPoints.Remove(spawnPoint);
                
                spawned.transform.position = spawnPoint.transform.position;
                
                var lever = spawned.GetComponent<Lever>();
                lever.OnLeverPressed += IncreaseProgress; // Expose in lever
            }
        }
    }
}