using DG.Tweening;
using UnityEngine;

namespace Core
{
    public sealed class ObjectShaker : MonoBehaviour
    {
        [SerializeField] private float strength;
        [SerializeField] private int vibrato;
        [SerializeField] private int randomness = 90;

        private void Start() => StartShakePosition();

        private void StartShakePosition() =>
            transform.DOShakePosition(10f, strength, vibrato, randomness, false, false, ShakeRandomnessMode.Harmonic)
                .onComplete += StartShakePosition;
    }
}