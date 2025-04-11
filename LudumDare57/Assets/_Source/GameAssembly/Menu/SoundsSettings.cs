using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Menu
{
    public sealed class SoundsSettings : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        [SerializeField] private Slider generalVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider soundsVolumeSlider;

        private void Start()
        {
            Bind();
            
            audioMixer.GetFloat("General", out var general);
            generalVolumeSlider.value = Mathf.Pow(10f, general / 20f);
            
            audioMixer.GetFloat("Music", out var music);
            musicVolumeSlider.value = Mathf.Pow(10f, music / 20f);
            
            audioMixer.GetFloat("Sounds", out var sounds);
            soundsVolumeSlider.value = Mathf.Pow(10f, sounds / 20f);
        }

        private void OnDestroy() => Expose();

        private void OnGeneralSliderChanged(float value) => audioMixer.SetFloat("General", Mathf.Log10(value) * 20);

        private void OnMusicSliderChanged(float value) => audioMixer.SetFloat("Music", Mathf.Log10(value) * 20);

        private void OnSoundsSliderChanged(float value) => audioMixer.SetFloat("Sounds", Mathf.Log10(value) * 20);

        private void Bind()
        {
            generalVolumeSlider.onValueChanged.AddListener(OnGeneralSliderChanged);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            soundsVolumeSlider.onValueChanged.AddListener(OnSoundsSliderChanged);
        }

        private void Expose()
        {
            generalVolumeSlider.onValueChanged.RemoveAllListeners();
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
            soundsVolumeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}