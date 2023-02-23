using System;
using UnityEngine;

public class AccessiblePlacesCountDisplay : PulsatingCountDisplay
{
    [SerializeField] private HuntersSquad _squad;

    protected override void SubscribeToEvents() 
        => _squad.AccessiblePlacesCountChanged += OnCountChanged;

    protected override void UnsubscribeFromEvents() 
        => _squad.AccessiblePlacesCountChanged -= OnCountChanged;

    protected override void ValidateAdditionalFields()
    {
        if (_squad == null)
            throw new InvalidOperationException();
    }
}