using System;
using System.Collections.Generic;
using UnityEngine;

public class TentBuilder : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _needSquadLevel;
    [SerializeField] private HuntersSquad _squad;
    [SerializeField] private HuntersSpawner _spawner;
    [SerializeField] private TentPurchaseZone _purchaseZone;
    [SerializeField] private List<BuildingPhase> _phases;

    private int _currentPhaseIndex;
    private int _buildingPhasePrice;

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

        _purchaseZone.PaymentAccepted += OnPaymentAccepted;
        _purchaseZone.Purchased += OnPurchased;
        _squad.LevelChanged += OnLevelChanged;
    }

    private void OnDisable()
    {
        _purchaseZone.PaymentAccepted -= OnPaymentAccepted;
        _purchaseZone.Purchased -= OnPurchased;
        _squad.LevelChanged -= OnLevelChanged;
    }

    private void Start()
    {
        _buildingPhasePrice = _purchaseZone.TotalPrice / _phases.Count;
        _purchaseZone.gameObject.SetActive(false);
    }

    private void OnPaymentAccepted()
    {
        if (_purchaseZone.RemainingPrice % _buildingPhasePrice == 0)
        {
            _phases[_currentPhaseIndex].Build();
            _currentPhaseIndex++;
        }
    }

    private void OnPurchased()
    {
        _spawner.enabled = true;
        Destroy(_purchaseZone.gameObject);
    }

    private void OnLevelChanged(int currentLevel)
    {
        if (currentLevel == _needSquadLevel)
            _purchaseZone.gameObject.SetActive(true);
    }

    private void Validate()
    {
        if (_squad == null)
            throw new InvalidOperationException();

        if (_spawner == null)
            throw new InvalidOperationException();

        if (_purchaseZone == null)
            throw new InvalidOperationException();

        if (_phases == null)
            throw new InvalidOperationException();

        if (_phases.Count == 0)
            throw new InvalidOperationException();

        foreach (BuildingPhase phase in _phases)
            if (phase == null)
                throw new InvalidOperationException();
    }
}