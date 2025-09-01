using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace TechArtProject
{
    public class ToggleView : MonoBehaviour
    {
        //Create logic for just showing off or on gameobject, with flag for setting default state
        [SerializeField] private Button _switchButton;
        [SerializeField] private Image _toggleOn;
        [SerializeField] private Image _toggleOff;
        [SerializeField] private bool _isOnWhenStart = true;
        [SerializeField] private float _tweenTime = 0.3f;
        [SerializeField] private Ease _tweenEase = Ease.Linear;
        [SerializeField] private Vector3 _tweenScale = Vector3.one;
        
        private Sequence _toggleSequence;

        private void Awake()
        {
            _switchButton.onClick.AddListener(() =>
            {
                _isOnWhenStart = !_isOnWhenStart;
                SetState(_isOnWhenStart);
            });
        }

        private void Start()
        {
            SetState(_isOnWhenStart);
        }

        private void SetState(bool isOnWhenStart)
        {
            if (_toggleSequence != null && _toggleSequence.IsActive())
                _toggleSequence.Kill();
            
            _toggleSequence = DOTween.Sequence();
            _toggleSequence.Join(_toggleOn.DOFade(isOnWhenStart ? 1f : 0f, _tweenTime).SetEase(_tweenEase));
            _toggleSequence.Join(_toggleOff.DOFade(isOnWhenStart ? 0f : 1f, _tweenTime).SetEase(_tweenEase));
            _toggleSequence.Join(
                gameObject.transform.DOScale(_tweenScale, _tweenTime * 0.5f)
                .SetEase(_tweenEase)
                .OnComplete(() => gameObject.transform.DOScale(Vector3.one, _tweenTime * 0.5f).SetEase(_tweenEase))
                );
        }
    }
}