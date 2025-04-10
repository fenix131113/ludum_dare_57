using UnityEngine;

namespace Menu
{
    public class BackgroundMove : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private RectTransform[] backs;

        private int _currentBackIndex;
        
        private void Update()
        {
            if (backs[_currentBackIndex].localPosition.y >= 2160)
            {
                backs[_currentBackIndex].transform.localPosition = new Vector3(
                    backs[_currentBackIndex].transform.localPosition.x, -1080f,
                    backs[_currentBackIndex].transform.localPosition.z);

                if (_currentBackIndex + 1 > backs.Length - 1)
                    _currentBackIndex = 0;
                else
                    _currentBackIndex++;
            }
            
            foreach (var back in backs)
                back.transform.position -= new Vector3(0, speed * Time.fixedDeltaTime, 0);
        }
    }
}