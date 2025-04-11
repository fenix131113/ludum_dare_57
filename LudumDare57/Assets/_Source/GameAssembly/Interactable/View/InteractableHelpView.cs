using System;
using DG.Tweening;
using Player;
using TMPro;
using UnityEngine;
using VContainer;

namespace Interactable.View
{
    public sealed class InteractableHelpView : MonoBehaviour
    {
        [SerializeField] private float animationDuration = 0.3f;
        [SerializeField] private float yOffset;
        [SerializeField] private TMP_Text helpText;

        private InteractiveChecker _checker;
        private Sequence _anim;
        private GameObject _currentTarget;

        [Inject]
        private void Construct(InteractiveChecker checker) => _checker = checker;

        private void Start()
        {
            Bind();
            helpText.transform.parent.parent = null;
        }

        private void OnDestroy() => Expose();

        private void Update()
        {
            if (_currentTarget)
                SetTextCanvasPosition(_currentTarget.transform.position);
        }

        private void OnInteractiveTargetChanged(GameObject target)
        {
            if (!target)
            {
                _currentTarget = null;
                _anim?.Kill();

                _anim = DOTween.Sequence();

                if (helpText.gameObject && helpText)
                {
                    _anim.Append(helpText.transform.DOLocalMoveY(0, animationDuration));
                    _anim.Insert(0, DOTween.To(() => helpText.color.a, SetTextFade, 0f, animationDuration));
                    _anim.onComplete += () => { helpText.gameObject.SetActive(false); };
                }
            }
            else
            {
                _currentTarget = target;
                SetTextCanvasPosition(_currentTarget.transform.position);
                SetTextFade(0f);

                if (helpText.gameObject && helpText)
                {
                    helpText.gameObject.SetActive(true);
                    helpText.transform.localPosition = Vector3.zero;

                    _anim?.Kill();

                    _anim = DOTween.Sequence();
                    _anim.Append(helpText.transform.DOLocalMoveY(helpText.transform.localPosition.y + yOffset,
                        animationDuration));
                    _anim.Insert(0, DOTween.To(() => helpText.color.a, SetTextFade, 1f, animationDuration));
                }
            }
        }

        private void SetTextCanvasPosition(Vector3 position) => helpText.transform.parent.position = position;

        private void SetTextFade(float value)
        {
            var color = helpText.color;
            color.a = value;
            helpText.color = color;
        }

        private void Bind() => _checker.OnInteractTargetChanged += OnInteractiveTargetChanged;

        private void Expose() => _checker.OnInteractTargetChanged -= OnInteractiveTargetChanged;
    }
}