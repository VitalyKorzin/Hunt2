using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextRipple))]
public abstract class PulsatingCountDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;

    private TextRipple _ripple;
    private int _currentValue;

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

        SubscribeToEvents();
    }

    private void OnDisable() 
        => UnsubscribeFromEvents();

    private void Awake()
    {
        _ripple = GetComponent<TextRipple>();
        _ripple.Initialize(_text);
    }

    protected virtual void SubscribeToEvents() { }

    protected virtual void UnsubscribeFromEvents() { }

    protected virtual void ValidateAdditionalFields() { }

    protected void OnCountChanged(int newValue)
    {
        if (_currentValue < newValue)
            _ripple.IncreaseText();
        else
            _ripple.DecreaseText();

        _currentValue = newValue;
        _text.text = _currentValue.ToString();
    }

    private void Validate()
    {
        if (_text == null)
            throw new InvalidOperationException();

        ValidateAdditionalFields();
    }
}