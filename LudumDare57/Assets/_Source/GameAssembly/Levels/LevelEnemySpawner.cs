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
        [SerializeField] private int maxCount;

        private int _currentEnemyCount;
        
        private void Start()
        {
            StartCoroutine(SpawnCoroutine());
        }

        private void OnEnemyDead(AEnemy enemy)
        {
            _currentEnemyCount--;
            enemy.OnDeath -= OnEnemyDead;
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                yield return new WaitUntil(() => _currentEnemyCount < maxCount);
                
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));

                var selectedEnemy = allowedEnemies[Random.Range(0, allowedEnemies.Count)];
                var spawned = GameInstaller.InstantiateInjectedObject(selectedEnemy.gameObject);
                spawned.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
                _currentEnemyCount++;
                spawned.GetComponent<AEnemy>().OnDeath += OnEnemyDead;
            }
            // ReSharper disable once IteratorNeverReturns
        }
    }
}