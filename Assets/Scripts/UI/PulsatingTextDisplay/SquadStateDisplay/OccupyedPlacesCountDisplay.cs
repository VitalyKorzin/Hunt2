using System;
using System.Collections.Generic;
using UnityEngine;

public class OccupyedPlacesCountDisplay : PulsatingCountDisplay
{
    [SerializeField] private List<HuntersSpawner> _spawners;

    protected override void SubscribeToEvents()
    {
        foreach (HuntersSpawner spawner in _spawners)
            spawner.Spawned += OnCountChanged;
    }

    protected override void UnsubscribeFromEvents()
    {
        foreach (HuntersSpawner spawner in _spawners)
            spawner.Spawned -= OnCountChanged;
    }

    protected override void ValidateAdditionalFields()
    {
        if (_spawners == null)
            throw new InvalidOperationException();

        if (_spawners.Count == 0)
            throw new InvalidOperationException();

        foreach (HuntersSpawner spawner in _spawners)
            if (spawner == null)
                throw new InvalidOperationException();
    }
}