using UnityEngine;

public class TentPurchaseZone : PurchaseZone
{
    protected override bool CanPull(Inventory inventory) 
        => inventory.CanPullBone();

    protected override Resource PullResource(Inventory inventory)
        => inventory.PullBone();
}