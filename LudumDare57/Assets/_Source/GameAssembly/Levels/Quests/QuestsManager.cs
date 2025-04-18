﻿using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Levels.Quests
{
    public class QuestsManager : MonoBehaviour
    {
        [SerializeField] private List<QuestSO> allQuests;
        [SerializeField] private List<QuestSetupPair> questSetups;
        [SerializeField] private List<Transform> specialItemsPoints;

        public QuestSO CurrentQuest { get; private set; }
        public int NeedValue { get; private set; }
        public int CurrentValue { get; private set; }
        public bool IsQuestComplete { get; private set; }

        public event Action OnQuestCompleted;
        public event Action OnQuestValueChange;

        public IReadOnlyCollection<Transform> GetSpecialItemsPoints() => specialItemsPoints;

        private PlayerStats _stats;

        [Inject]
        private void Construct(PlayerStats stats) => _stats = stats;

        public void Awake() => LevelInitialize();

        private void LevelInitialize()
        {
            CurrentQuest = allQuests[Random.Range(0, allQuests.Count)];

            NeedValue = Random.Range(CurrentQuest.NeedValueMin, CurrentQuest.NeedValueMax + 1);

            var setup = questSetups.First(x => x.Quest == CurrentQuest).QuestSetupBase;
            setup.gameObject.SetActive(true);
            setup.Setup(this);
        }

        public void SetQuestProgressValue(int value)
        {
            CurrentValue = value;

            OnQuestValueChange?.Invoke();
            
            IsQuestComplete = value >= NeedValue;
            
            if (!IsQuestComplete)
                return;
            
            OnQuestCompleted?.Invoke();
        }

        public void GiveReward() => _stats.AddCoins(questSetups.First(x => x.Quest == CurrentQuest).QuestSetupBase.GetReward());

        [Serializable]
        public class QuestSetupPair
        {
            [field: SerializeField] public QuestSO Quest { get; private set; }
            [field: SerializeField] public AQuestSetupBase QuestSetupBase { get; private set; }
        }
    }
}