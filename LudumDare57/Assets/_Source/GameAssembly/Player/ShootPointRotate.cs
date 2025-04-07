using UnityEngine;

namespace Player
{
    public sealed class ShootPointRotate : MonoBehaviour
    {
        [SerializeField] private Transform sightCenter;
        [SerializeField] private SpriteRenderer playerRenderer;

        private void Update()
        {
            var mouseWorldPos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var direction = mouseWorldPos - sightCenter.position;
            direction.z = 0f;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            transform.localScale = angle is > 90 or < -90 ?
                new Vector3(1f, -1f, 1f)
                : new Vector3(1f, 1f, 1f);

            playerRenderer.flipX = angle is > 90 or < -90;
                

            sightCenter.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}