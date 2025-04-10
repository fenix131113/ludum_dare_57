using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Menu
{
    public class BoxesTrigger : MonoBehaviour
    {
        [SerializeField] private Transform rightPoint;
        [SerializeField] private Transform leftPoint;
        [SerializeField] private float minTimeToSpawn;
        [SerializeField] private float maxTimeToSpawn;
        [SerializeField] private List<GameObject> boxes;

        private void Start() => StartCoroutine(SpawnBoxCoroutine());

        private void OnTriggerEnter2D(Collider2D other)
        {
            boxes.Add(other.gameObject);
            other.gameObject.SetActive(false);
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private IEnumerator SpawnBoxCoroutine()
        {
            yield return new WaitForSeconds(Random.Range(minTimeToSpawn, maxTimeToSpawn));
            
            if (boxes.Count == 0)
                StartCoroutine(SpawnBoxCoroutine());

            var box = boxes[Random.Range(0, boxes.Count)];
            boxes.Remove(box);
            box.SetActive(true);
            
            box.transform.position = new Vector3(Random.Range(leftPoint.position.x, rightPoint.position.x),
                rightPoint.position.y, box.transform.position.z);
            
            box.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            box.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            
            StartCoroutine(SpawnBoxCoroutine());
        }
    }
}