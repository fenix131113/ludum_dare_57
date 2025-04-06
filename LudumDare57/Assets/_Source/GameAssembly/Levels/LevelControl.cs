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

        private void Start() => DontDestroyOnLoad(gameObject);

        public void LoadNextRandomLevel()
        {
            var nextSceneIndex =
                allowGameScenesIndexes.Except(new[] { SceneManager.GetActiveScene().buildIndex }).ToList()[
                    Random.Range(0, allowGameScenesIndexes.Count)];

            SceneManager.LoadScene(nextSceneIndex);
        }

        public void LoadShopLevel() => SceneManager.LoadScene(shopSceneIndex);
    }
}