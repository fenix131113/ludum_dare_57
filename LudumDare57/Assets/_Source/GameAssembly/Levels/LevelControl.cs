using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using Random = UnityEngine.Random;

namespace Levels
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private List<LevelSpawnData> levelsSpawnData;
        [SerializeField] private int shopSceneIndex;

        public static LevelControl Instance;

        private void Start()
        {
            if (Instance && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            GameInstaller.InjectObject(gameObject);
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void LoadNextRandomLevel(bool includeCurrent)
        {
            int nextSceneIndex;

            var allowGameScenesIndexes =
                playerStats ? GetAllowGameScenesIndexes() : levelsSpawnData[0].LevelIndexes;

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
                if (data.LoadAfter > playerStats.CompletedLevels)
                    continue;

                allowed = data.LevelIndexes;
                break;
            }

            return allowed;
        }


        public void LoadShopLevel()
        {
            SceneManager.LoadScene(shopSceneIndex);
        }

        [Serializable]
        public class LevelSpawnData
        {
            [field: SerializeField] public int LoadAfter { get; private set; }
            [field: SerializeField] public List<int> LevelIndexes { get; private set; }
        }
    }
}