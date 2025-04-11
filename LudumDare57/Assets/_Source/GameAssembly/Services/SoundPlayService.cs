using UnityEngine;

namespace Services
{
    public class SoundPlayService : MonoBehaviour
    {
        public static SoundPlayService Instance;

        [SerializeField] private AudioSource soundSource;

        private void Awake() => Instance = this;

        public void PlaySound(AudioClip clip, float volume = 1f) => soundSource.PlayOneShot(clip, volume);
    }
}