using System;
using System.Collections;
using UnityEngine;

public class HeroRotator : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _speed;
    [SerializeField] private HeroMover _mover;
    [SerializeField] private HeroAttacker _attacker;

    private Coroutine _rotationJob;

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

        _attacker.AttackBegun += OnAttackBegun;
        _attacker.AttackIsOver += OnAttackIsOver;
    }

    private void OnDisable()
    {
        _attacker.AttackBegun -= OnAttackBegun;
        _attacker.AttackIsOver -= OnAttackIsOver;
    }

    private void Start()
    {
        _rotationJob = StartCoroutine(Rotate());
    }

    private void OnAttackBegun(Animal target)
    {
        StopRotationJob();
        _rotationJob = StartCoroutine(RotateTo(target));
    }

    private void OnAttackIsOver()
    {
        StopRotationJob();
        _rotationJob = StartCoroutine(Rotate());
    }

    private void StopRotationJob()
    {
        if (_rotationJob != null)
            StopCoroutine(_rotationJob);
    }

    private IEnumerator Rotate()
    {
        while (enabled != false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_mover.Duration), _speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator RotateTo(Animal target)
    {
        Vector3 duration;

        while (target != null)
        {
            duration = target.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(duration), _speed * Time.deltaTime);
            yield return null;
        }

        _rotationJob = StartCoroutine(Rotate());
    }

    private void Validate()
    {
        if (_mover == null)
            throw new InvalidOperationException();

        if (_attacker == null)
            throw new InvalidOperationException();
    }
}