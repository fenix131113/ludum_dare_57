using System.Collections;
using UnityEngine;

namespace Services
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner _instance;
        public static CoroutineRunner Instance
        {
            get
            {
                if (_instance)
                    return _instance;
                
                var go = new GameObject("CoroutineRunner");
                DontDestroyOnLoad(go);
                _instance = go.AddComponent<CoroutineRunner>();
                return _instance;
            }
        }

        public static void Run(IEnumerator coroutine) => Instance.StartCoroutine(coroutine);
    }
}