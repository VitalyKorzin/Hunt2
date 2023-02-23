using System;
using UnityEngine;

public abstract class ResourcesCountDisplay : PulsatingCountDisplay
{
    [SerializeField] private Inventory _inventory;

    protected abstract void SubscribeToEvent(Inventory inventory);

    protected abstract void UnsubscribeFromEvent(Inventory inventory);

    protected override void SubscribeToEvents() 
        => SubscribeToEvent(_inventory);

    protected override void UnsubscribeFromEvents() 
        => UnsubscribeFromEvent(_inventory);

    protected override void ValidateAdditionalFields()
    {
        if (_inventory == null)
            throw new InvalidOperationException();
    }
}