using System.Collections;
using System.Collections.Generic;
using Core;
using Enemies;
using UnityEngine;

namespace Levels
{
    public class LevelEnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<AEnemy> allowedEnemies;
        [SerializeField] private List<Transform> spawnPoints;
        [SerializeField] private float minTimeBetweenSpawns;
        [SerializeField] private float maxTimeBetweenSpawns;

        private void Start()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
                
                var spawned = GameInstaller.InstantiateInjectedObject(allowedEnemies[Random.Range(0, allowedEnemies.Count)].gameObject);
                spawned.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}