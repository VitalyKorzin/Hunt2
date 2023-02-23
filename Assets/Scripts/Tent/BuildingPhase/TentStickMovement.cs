using System;
using UnityEngine;
using DG.Tweening;

public class TentStickMovement : BuildingPhase
{
    [Min(0)]
    [SerializeField] private float _duration;
    [SerializeField] private TentStick _stick;
    [SerializeField] private Vector3 _targetPosition;

    private void OnEnable()
    {
        if (_stick == null)
        {
            enabled = false;
            throw new InvalidOperationException();
        }
    }

    private void Start() 
        => _stick.gameObject.SetActive(false);

    public override void Build()
    {
        _stick.gameObject.SetActive(true);
        _stick.transform.DOLocalMove(_targetPosition, _duration);
    }
}