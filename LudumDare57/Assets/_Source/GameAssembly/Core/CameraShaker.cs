using Unity.Cinemachine;
using UnityEngine;

namespace Core
{
    public class CameraShaker : MonoBehaviour
    {
        public static CameraShaker Instance;

        private CinemachineBasicMultiChannelPerlin _noise;
        private float _shakeTimer;

        private void Awake()
        {
            _noise = GetComponent<CinemachineBasicMultiChannelPerlin>();
            Instance = this;
        }

        private void Update()
        {
            if (_shakeTimer <= 0)
                return;
            
            _shakeTimer -= Time.deltaTime;
            if (_shakeTimer <= 0f)
                StopShake();
        }

        public void Shake(float shakeAmplitude, float shakeFrequency, float duration)
        {
            _noise.AmplitudeGain = shakeAmplitude;
            _noise.FrequencyGain = shakeFrequency;
            _shakeTimer = duration;
        }

        public void StopShake()
        {
            _noise.AmplitudeGain = 0f;
            _noise.FrequencyGain = 0f;
        }
    }
}