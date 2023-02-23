using UnityEngine;

public class MeatsCountDisplay : ResourcesCountDisplay
{
    protected override void SubscribeToEvent(Inventory inventory) 
        => inventory.MeatsCountChanged += OnCountChanged;

    protected override void UnsubscribeFromEvent(Inventory inventory) 
        => inventory.MeatsCountChanged -= OnCountChanged;
}