using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class HuntStartAnnunciatorDisplay : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _appearanceDuration;
    [Min(0)]
    [SerializeField] private float _fadingDuration;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _text;

    private readonly float _appearanceEndValue = 1f;
    private readonly float _fadingEndValue = 0f;
    private readonly float _appearanceScale = 1f;
    private readonly float _fadingScale = 0f;

    private void OnEnable()
    {
        try
        {
            Validate();
        }
        catch (Exception exception)
        {
            enabled = false;
            throw exception;
        }
    }

    public void Appear()
    {
        transform.DOScale(_appearanceScale, _appearanceDuration);
        _icon.DOFade(_appearanceEndValue, _appearanceDuration);
        _text.DOFade(_appearanceEndValue, _fadingDuration);
    }

    public void Fade()
    {
        transform.DOScale(_fadingScale, _fadingDuration);
        _icon.DOFade(_fadingEndValue, _fadingDuration);
        _text.DOFade(_fadingEndValue, _fadingDuration);
    }

    private void Validate()
    {
        if (_icon == null)
            throw new InvalidOperationException();

        if (_text == null)
            throw new InvalidOperationException();
    }
}