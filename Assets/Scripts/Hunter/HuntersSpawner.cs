using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HuntersSpawner : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _secondsBetweenSpawn;
    [SerializeField] private Hunter _template;
    [SerializeField] private HuntersSquad _squad;
    [SerializeField] private Transform _spawnPoint;

    private int _spawnedCount;

    public event UnityAction<int> Spawned;

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

        _squad.Upgraded += OnUpgraded;
    }

    private void OnDisable() 
        => _squad.Upgraded -= OnUpgraded;

    private void OnUpgraded()
    {
        if (_spawnedCount < _squad.GetOccupiedPlacesCount())
            return;

        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        var delay = new WaitForSeconds(_secondsBetweenSpawn);

        while (_squad.CanOccupyAccessiblePlace())
        {
            PlaceInSquad place = _squad.OccupyAccessiblePlace();
            yield return delay;
            Hunter hunter = Instantiate(_template, _spawnPoint.position, _spawnPoint.rotation);
            hunter.Initialize(place.transform);
            _squad.Add(hunter);
            _spawnedCount++;
            Spawned?.Invoke(_spawnedCount);
        }
    }

    private void Validate()
    {
        if (_template == null)
            throw new InvalidOperationException();

        if (_squad == null)
            throw new InvalidOperationException();

        if (_spawnPoint == null)
            throw new InvalidOperationException();
    }
}