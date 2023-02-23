using System;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(RectTransform))]
public class ButtonDisplay : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _secondsBeforeAppearance;
    [Min(0)]
    [SerializeField] private float _movementDuration;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _target;
    [SerializeField] private Fauna _fauna;

    private RectTransform _rectTransform;

    private void OnEnable()
    {
        if (_fauna == null)
        {
            enabled = false;
            throw new InvalidOperationException();
        }

        _fauna.Died += OnFaunaDied;
    }

    private void OnDisable() 
        => _fauna.Died -= OnFaunaDied;

    private void Start() 
        => _rectTransform = GetComponent<RectTransform>();

    private void OnFaunaDied()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_rectTransform.DOAnchorPos(_offset, _movementDuration / 2f)).SetDelay(_secondsBeforeAppearance);
        sequence.Append(_rectTransform.DOAnchorPos(_target, _movementDuration / 2f));
    }
}