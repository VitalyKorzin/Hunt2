using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class HunterMover : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _targetDistanceToAnimal;

    private readonly float _targetDistanceToPlaceInSquad = 0.1f;

    private Coroutine _movementJob;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    public event UnityAction<Animal> TargetApproached;
    public event UnityAction<Transform> CameToPlaceInSquad;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    public void StartMove(Animal target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        StartMove(Move(target));
    }

    public void StartMove(Transform target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        StartMove(Move(target));
    }

    private void StartMove(IEnumerator movementJob)
    {
        if (_movementJob != null)
            StopCoroutine(_movementJob);

        _animator.SetBool(HunterAnimator.Params.IsRunning, true);
        _movementJob = StartCoroutine(movementJob);
    }

    private IEnumerator Move(Transform target)
    {
        yield return Move(target, _targetDistanceToPlaceInSquad);
        CameToPlaceInSquad?.Invoke(target);
    }

    private IEnumerator Move(Animal target)
    {
        yield return Move(target.transform, _targetDistanceToAnimal);
        TargetApproached?.Invoke(target);
    }

    private IEnumerator Move(Transform target, float targetDistance)
    {
        _navMeshAgent.isStopped = false;

        while (Vector3.Distance(transform.position, target.position) > targetDistance)
        {
            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.SetDestination(target.position);

            yield return null;
        }

        _animator.SetBool(HunterAnimator.Params.IsRunning, false);
        _navMeshAgent.isStopped = true;
    }
}