using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Levels
{
    public class LevelControl : MonoBehaviour
    {
        [SerializeField] private List<int> allowGameScenesIndexes = new();
        [SerializeField] private int shopSceneIndex;

        public static LevelControl Instance;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }

        public void LoadNextRandomLevel(bool includeCurrent)
        {
            int nextSceneIndex;

            if (includeCurrent)
                nextSceneIndex = allowGameScenesIndexes[Random.Range(0, allowGameScenesIndexes.Count)];
            else
                nextSceneIndex = allowGameScenesIndexes.Except(new[] { SceneManager.GetActiveScene().buildIndex })
                    .ToList()[
                        Random.Range(0, allowGameScenesIndexes.Count)];

            SceneManager.LoadScene(nextSceneIndex);
        }

        public void LoadShopLevel() => SceneManager.LoadScene(shopSceneIndex);
    }
}