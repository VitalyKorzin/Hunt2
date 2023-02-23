using System;
using UnityEngine;
using DG.Tweening;

public class TentScaling : BuildingPhase
{
    [Min(0)]
    [SerializeField] private float _duration;
    [SerializeField] private Vector3 _targetScale;
    [SerializeField] private Tent _tent;

    private void OnEnable()
    {
        if (_tent == null)
        {
            enabled = false;
            throw new InvalidOperationException();
        }
    }

    public override void Build() 
        => _tent.transform.DOScale(_targetScale, _duration);
}