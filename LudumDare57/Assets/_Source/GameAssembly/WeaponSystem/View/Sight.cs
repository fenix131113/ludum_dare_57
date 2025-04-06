using UnityEngine;

namespace WeaponSystem.View
{
    public class Sight : MonoBehaviour
    {
        private void OnEnable()
        {
            Cursor.visible = false;
        }

        public void ActivateSight()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateSight()
        {
            gameObject.SetActive(false);
            Cursor.visible = true;
        }

        private void Update()
        {
            transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                Mathf.Clamp(Input.mousePosition.y, 0, Screen.height),
                transform.position.z);
        }
    }
}