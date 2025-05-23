﻿using UnityEngine;

namespace WeaponSystem.View
{
    public sealed class Sight : MonoBehaviour
    {
        private void Update()
        {
            transform.position = new Vector3(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
                Mathf.Clamp(Input.mousePosition.y, 0, Screen.height),
                transform.position.z);
        }
    }
}