using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TextRipple : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _duration;
    [Min(0)]
    [SerializeField] private float _scaleAsValueIncreases;
    [Min(0)]
    [SerializeField] private float _scaleAsValueDecreases;
    [Min(0)]
    [SerializeField] private float _defaultScale;
    [SerializeField] private Vector3 _offsetAsValueIncreases;
    [SerializeField] private Vector3 _offsetAsValueDecreases;
    [SerializeField] private Vector3 _defaultPosition;

    private TMP_Text _text;
    private float _stepDuration;

    public void Initialize(TMP_Text text)
    {
        if (text == null)
            throw new ArgumentNullException();

        _text = text;
        _stepDuration = _duration / 2f;
    }

    public void IncreaseText()
    {
        ChangeScaleText(_scaleAsValueIncreases);
        MoveText(_offsetAsValueIncreases);
    }

    public void DecreaseText()
    {
        ChangeScaleText(_scaleAsValueDecreases);
        MoveText(_offsetAsValueDecreases);
    }

    private void ChangeScaleText(float scale)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_text.rectTransform.DOScale(scale, _stepDuration));
        sequence.Append(_text.rectTransform.DOScale(_defaultScale, _stepDuration));
    }

    private void MoveText(Vector3 offset)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_text.rectTransform.DOAnchorPos(offset, _stepDuration));
        sequence.Append(_text.rectTransform.DOAnchorPos(_defaultPosition, _stepDuration));
    }
}