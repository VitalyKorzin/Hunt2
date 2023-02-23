using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesShooter : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _resourcesCount;
    [Min(0)]
    [SerializeField] private float _radius;
    [Range(0, 90)]
    [SerializeField] private float _minimumAngle;
    [Range(0, 90)]
    [SerializeField] private float _maximumAngle;
    [SerializeField] private ResourcesDispenser _dispenser;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private Animal _animal;

    private readonly float _timeStep = 0.01f;

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

        _animal.Died += OnAnimalDied;
    }

    private void OnDisable() 
        => _animal.Died -= OnAnimalDied;

    private void Start()
    {
        transform.parent = _animal.transform;
        transform.localPosition = Vector3.zero;
    }

    private void OnAnimalDied(Animal animal)
    {
        List<Resource> resources = _dispenser.Dispense(transform.position, _resourcesCount);

        foreach (Resource resource in resources)
            Shot(resource);
    }

    private void Shot(Resource resource)
    {
        _shotPoint.position = new Vector3(_shotPoint.position.x, resource.OffsetY + 0.2f, _shotPoint.position.z);
        Vector3 targetPosition = GetRandomTargetPosition(resource);
        Vector3 direction = targetPosition - transform.position;
        Vector3 directionXZ = new Vector3(direction.x, 0f, direction.z);
        transform.rotation = Quaternion.LookRotation(directionXZ, Vector3.up);
        float randomAngle = UnityEngine.Random.Range(_minimumAngle, _maximumAngle);
        _shotPoint.localEulerAngles = new Vector3(-randomAngle, 0f, 0f);
        float x = directionXZ.magnitude;
        float y = direction.y;
        float angleInRadians = randomAngle * Mathf.Deg2Rad;
        float velocity2 = Physics.gravity.y * Mathf.Pow(x, 2) / (2 * (y - Mathf.Tan(angleInRadians) * x) * Mathf.Pow(Mathf.Cos(angleInRadians), 2));
        float velocity = Mathf.Sqrt(Mathf.Abs(velocity2));
        Vector3 vector = _shotPoint.forward * velocity;
        resource.StartMove(GetPath(vector, targetPosition.y));
    }

    private Vector3[] GetPath(Vector3 vector, float targetPositionY)
    {
        List<Vector3> path = new List<Vector3>();
        float time = path.Count * _timeStep;
        path.Add(_shotPoint.position + vector * time + Physics.gravity * Mathf.Pow(time, 2) / 2f);

        while (path[path.Count - 1].y > targetPositionY)
        {
            time = path.Count * _timeStep;
            path.Add(_shotPoint.position + vector * time + Physics.gravity * Mathf.Pow(time, 2) / 2f);
        }

        return path.ToArray();
    }

    private Vector3 GetRandomTargetPosition(Resource resource)
    {
        float x = UnityEngine.Random.Range(-_radius + transform.position.x, _radius + transform.position.x);
        float y = resource.OffsetY;
        float z = UnityEngine.Random.Range(-_radius + transform.position.z, _radius + transform.position.z);
        return new Vector3(x, y, z);
    }

    private void Validate()
    {
        if (_dispenser == null)
            throw new InvalidOperationException();

        if (_shotPoint == null)
            throw new InvalidOperationException();

        if (_animal == null)
            throw new InvalidOperationException();
    }
}