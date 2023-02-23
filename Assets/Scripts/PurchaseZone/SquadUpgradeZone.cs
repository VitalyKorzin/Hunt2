using UnityEngine;

public class SquadUpgradeZone : PurchaseZone
{
    protected override bool CanPull(Inventory inventory)
        => inventory.CanPullMeat();

    protected override Resource PullResource(Inventory inventory)
        => inventory.PullMeat();
}