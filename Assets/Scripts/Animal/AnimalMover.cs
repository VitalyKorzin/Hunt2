using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public class AnimalMover : MonoBehaviour
{
    [SerializeField] private Transform[] _path;

    private readonly float _targetDisptance = 0.6f;

    private Coroutine _walkingJob;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private int _currentWaypointIndex = 0;

    private void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _walkingJob = StartCoroutine(Walk());
    }

    public void StopWalk()
    {
        if (_walkingJob != null)
            StopCoroutine(_walkingJob);

        _animator.SetBool(AnimalAnimator.Params.IsWalking, false);
        _navMeshAgent.enabled = false;
    }

    private IEnumerator Walk()
    {
        _animator.SetBool(AnimalAnimator.Params.IsWalking, true);
        Vector3 targetPosition = _path[_currentWaypointIndex].position;

        while (Vector3.Distance(transform.position, targetPosition) > _targetDisptance)
        {
            if (_navMeshAgent.isActiveAndEnabled)
                _navMeshAgent.SetDestination(targetPosition);

            yield return null;
        }

        _animator.SetBool(AnimalAnimator.Params.IsWalking, false);
        ChangeCurrentWaypointIndex();
        _walkingJob = StartCoroutine(Walk());
    }

    private void ChangeCurrentWaypointIndex()
    {
        _currentWaypointIndex++;

        if (_currentWaypointIndex == _path.Length)
            _currentWaypointIndex = 0;
    }
}