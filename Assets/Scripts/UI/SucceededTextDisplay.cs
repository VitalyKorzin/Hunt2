using System;
using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class SucceededTextDisplay : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _appearanceDuration;
    [Range(0, 1)]
    [SerializeField] private float _appearanceEndValue;
    [SerializeField] private Fauna _fauna;
    [SerializeField] private Vector3 _targetScale;

    private TMP_Text _text;

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
        => _text = GetComponent<TMP_Text>();

    private void OnFaunaDied()
    {
        _text.DOFade(_appearanceEndValue, _appearanceDuration);
        transform.DOScale(_targetScale, _appearanceDuration);
    }
}