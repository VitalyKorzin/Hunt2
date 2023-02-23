using UnityEngine;

public class BonesCountDisplay : ResourcesCountDisplay
{
    protected override void SubscribeToEvent(Inventory inventory) 
        => inventory.BonesCountChanged += OnCountChanged;

    protected override void UnsubscribeFromEvent(Inventory inventory) 
        => inventory.BonesCountChanged -= OnCountChanged;
}