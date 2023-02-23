using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(HunterMover), typeof(Animator))]
public class HunterAttacker : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _secondsBetweenAttack;
    [SerializeField] private Transform _spearPosition;
    [SerializeField] private Transform _ropePosition;
    [SerializeField] private Spear _spearTemplate;
    [SerializeField] private Rope _ropeTemplate;

    private HunterMover _mover;
    private Animator _animator;
    private Coroutine _attackingJob;
    private Spear _currentSpear;
    private Rope _currentRope;

    public event UnityAction TargetDied;

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
    }

    private void OnDisable() 
        => _mover.TargetApproached -= OnTargetApproached;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<HunterMover>();
        _mover.TargetApproached += OnTargetApproached;
        CreateSpear();
    }

    private void OnTargetApproached(Animal target)
    {
        target.Died += OnTargetDied;

        if (_attackingJob != null)
            StopCoroutine(_attackingJob);

        _attackingJob = StartCoroutine(Attack(target));
    }

    private void OnTargetDied(Animal target)
    {
        target.Died -= OnTargetDied;

        if (_attackingJob != null)
            StopCoroutine(_attackingJob);

        if (_currentRope != null)
            Destroy(_currentRope.gameObject);

        CreateSpear();
        TargetDied?.Invoke();
    }

    private IEnumerator Attack(Animal target)
    {
        var delayBeforeThrowing = new WaitForSeconds(_secondsBetweenAttack / 2f);
        var delayBeforeSwing = new WaitForSeconds(_secondsBetweenAttack / 2f);

        while (target.Alive)
        {
            _animator.SetTrigger(HunterAnimator.Params.Attacked);
            yield return delayBeforeThrowing;
            CreateSpear();
            _currentSpear.transform.parent = null;
            _currentSpear.Throw(target.SpearTarget);
            CreateRope();
            yield return delayBeforeSwing;
        }
    }

    private void CreateSpear()
    {
        if (_currentSpear != null)
            Destroy(_currentSpear.gameObject);

        _currentSpear = Instantiate(_spearTemplate, _spearPosition);
    }

    private void CreateRope()
    {
        if (_currentRope != null)
            Destroy(_currentRope.gameObject);

        _currentRope = Instantiate(_ropeTemplate, transform);
        _currentRope.Initialize(_ropePosition, _currentSpear.transform);
    }

    private void Validate()
    {
        if (_spearPosition == null)
            throw new InvalidOperationException();

        if (_ropePosition == null)
            throw new InvalidOperationException();

        if (_spearTemplate == null)
            throw new InvalidOperationException();

        if (_ropeTemplate == null)
            throw new InvalidOperationException();
    }
}