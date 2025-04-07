using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Services
{
    public class FadeService : MonoBehaviour
    {
        private const string FADE_CANVAS_PATH = "Prefabs/Ui/FadeCanvas";

        private static FadeService _instance;

        private Image _fadeImage;
        private Tween _fadeTween;

        public static FadeService Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = Instantiate(Resources.Load(FADE_CANVAS_PATH) as GameObject).GetComponent<FadeService>();

                return _instance;
            }
        }

        private void LoadImage() => _fadeImage = transform.GetChild(0).GetComponent<Image>();

        public void FadeIn(float duration, Action onFinish = null)
        {
            if (!_fadeImage)
                LoadImage();

            if (!_fadeImage || (_fadeImage && _fadeTween.IsActive()))
                return;

            var color = _fadeImage.color;
            color.a = 0;
            _fadeImage.color = color;

            _fadeTween = _fadeImage.DOFade(1f, duration);
            _fadeTween.onComplete = () => onFinish?.Invoke();
        }

        public void FadeOut(float duration, Action onFinish = null)
        {
            if (!_fadeImage)
                LoadImage();

            if (!_fadeImage || (_fadeImage && _fadeTween.IsActive()))
                return;

            var color = _fadeImage.color;
            color.a = 1;
            _fadeImage.color = color;

            _fadeTween = _fadeImage.DOFade(0f, duration);
            _fadeTween.onComplete = () => onFinish?.Invoke();
        }
    }
}