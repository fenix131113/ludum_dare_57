using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using NUnit.Framework;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using Random = UnityEngine.Random;

namespace Levels
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] private List<LevelSpawnData> levelsSpawnData;
        [SerializeField] private int shopSceneIndex;

        public static LevelControl Instance;

        private PlayerStats _playerStats;

        [Inject]
        private void Construct(PlayerStats playerStats) => _playerStats = playerStats;

        private void Start()
        {
            GameInstaller.InjectObject(this);
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void LoadNextRandomLevel(bool includeCurrent)
        {
            int nextSceneIndex;

            var allowGameScenesIndexes =
                _playerStats ? GetAllowGameScenesIndexes() : levelsSpawnData[0].LevelIndexes;

            if (includeCurrent)
                nextSceneIndex = allowGameScenesIndexes[Random.Range(0, allowGameScenesIndexes.Count)];
            else
                nextSceneIndex = allowGameScenesIndexes.Except(new[] { SceneManager.GetActiveScene().buildIndex })
                    .ToList()[
                        Random.Range(0, allowGameScenesIndexes.Count)];

            SceneManager.LoadScene(nextSceneIndex);
        }

        private List<int> GetAllowGameScenesIndexes()
        {
            var allowed = new List<int>();

            foreach (var data in levelsSpawnData.OrderByDescending(x => x.LoadAfter))
            {
                if (data.LoadAfter > _playerStats.CompletedLevels)
                    continue;

                allowed = data.LevelIndexes;
                Debug.Log(data.LevelIndexes.Count);
                break;
            }

            return allowed;
        }

        public void LoadShopLevel() => SceneManager.LoadScene(shopSceneIndex);

        [Serializable]
        public class LevelSpawnData
        {
            [field: SerializeField] public int LoadAfter { get; private set; }
            [field: SerializeField] public List<int> LevelIndexes { get; private set; }
        }
    }
}