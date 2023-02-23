using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HunterMover))]
public class HunterRotator : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _speed;

    private HunterMover _mover;
    private Coroutine _rotationJob;

    private void OnDisable()
    {
        _mover.CameToPlaceInSquad -= OnCameToPlaceInSquad;
        _mover.TargetApproached -= OnTargetApproached;
    }

    private void Start()
    {
        _mover = GetComponent<HunterMover>();
        _mover.CameToPlaceInSquad += OnCameToPlaceInSquad;
        _mover.TargetApproached += OnTargetApproached;
    }

    private void OnCameToPlaceInSquad(Transform place)
    {
        if (_rotationJob != null)
            StopCoroutine(_rotationJob);

        _rotationJob = StartCoroutine(Rotate(place));
    }

    private void OnTargetApproached(Animal target)
    {
        if (_rotationJob != null)
            StopCoroutine(_rotationJob);

        _rotationJob = StartCoroutine(Rotate(target));
    }

    private IEnumerator Rotate(Transform place)
    {
        while (transform.rotation != place.rotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, place.rotation, _speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Rotate(Animal target)
    {
        while (target != null && transform.rotation != GetTargetRotation(target.transform.position))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, GetTargetRotation(target.transform.position), _speed * Time.deltaTime);
            yield return null;
        }
    }

    private Quaternion GetTargetRotation(Vector3 targetPosition)
        => Quaternion.LookRotation(targetPosition - transform.position);
}