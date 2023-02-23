using System;
using TMPro;
using UnityEngine;

public class SquadLevelDisplay : MonoBehaviour
{
    [SerializeField] private HuntersSquad _squad;
    [SerializeField] private TMP_Text _text;

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

        _squad.LevelChanged += OnLevelChanged;
    }

    private void OnDisable() 
        => _squad.LevelChanged -= OnLevelChanged;

    private void OnLevelChanged(int currentLevel) 
        => _text.text = currentLevel.ToString();

    private void Validate()
    {
        if (_squad == null)
            throw new InvalidOperationException();

        if (_text == null)
            throw new InvalidOperationException();
    }
}