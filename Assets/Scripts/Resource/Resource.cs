using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Resource : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _movementSpeed;
    [Min(0)]
    [SerializeField] private float _fallingSpeed;
    [Min(0)]
    [SerializeField] private float _offsetY;

    private Coroutine _movementJob;
    private int _currentWaypointIndex = 0;

    public float OffsetY => _offsetY;

    public event UnityAction<Resource> ReachedTargetPosition;

    public void StartMove(Vector3[] path)
    {
        if (path == null)
            throw new ArgumentNullException(nameof(path));

        if (path.Length == 0)
            throw new ArgumentOutOfRangeException(nameof(path));

        if (_movementJob != null)
            StopCoroutine(_movementJob);

        _movementJob = StartCoroutine(Move(path));
    }

    public void StartMove(Transform target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        if (_movementJob != null)
            StopCoroutine(_movementJob);

        _movementJob = StartCoroutine(Move(target));
    }

    private IEnumerator Move(Transform target)
    {
        while (target != null && target.position != transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, _movementSpeed * Time.deltaTime);
            yield return null;
        }

        transform.localRotation = target.localRotation;
        ReachedTargetPosition?.Invoke(this);
    }

    private IEnumerator Move(Vector3[] path)
    {
        while (_currentWaypointIndex < path.Length)
        {
            Vector3 targetPosition = path[_currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _fallingSpeed * Time.deltaTime);

            if (transform.position == targetPosition)
                _currentWaypointIndex++;

            yield return null;
        }
    }
}