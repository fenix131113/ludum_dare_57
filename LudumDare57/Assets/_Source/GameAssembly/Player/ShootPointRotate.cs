using UnityEngine;

namespace Player
{
    public class ShootPointRotate : MonoBehaviour
    {
        [SerializeField] private Transform sightCenter;

        private void Update()
        {
            var mouseWorldPos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var direction = mouseWorldPos - sightCenter.position;
            direction.z = 0f;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            sightCenter.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}