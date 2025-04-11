using UnityEngine;

namespace Menu
{
    public class BackgroundMove : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float upperEdge;
        [SerializeField] private float downSpawnCoords;
        [SerializeField] private Transform[] backs;

        private int _currentBackIndex;
        
        private void Update()
        {
            foreach (var back in backs)
                back.transform.position -= new Vector3(0, speed * Time.fixedDeltaTime, 0);
            
            if (backs[_currentBackIndex].localPosition.y >= upperEdge)
            {
                backs[_currentBackIndex].transform.localPosition = new Vector3(
                    backs[_currentBackIndex].transform.localPosition.x, downSpawnCoords + 0.09f,
                    backs[_currentBackIndex].transform.localPosition.z);

                if (_currentBackIndex + 1 > backs.Length - 1)
                    _currentBackIndex = 0;
                else
                    _currentBackIndex++;
            }
        }
    }
}