using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class Slot<T> : MonoBehaviour
    where T: Resource
{
    [Min(0)]
    [SerializeField] private float _distanceBetweenResources;
    [Min(0)]
    [SerializeField] private float _scaleMultiplier;
    [SerializeField] private PlaceInSlot _placeTemplate;

    private readonly List<T> _resources = new List<T>();
    private readonly List<PlaceInSlot> _places = new List<PlaceInSlot>();

    public int Count => _resources.Count;

    private void OnEnable()
    {
        if (_placeTemplate == null)
        {
            enabled = false;
            throw new InvalidOperationException();
        }
    }

    public void Push(T resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        CreateNewPlace();
        ChangeLastPlacePosition(resource.transform.lossyScale.y);
        _resources.Add(resource);
        resource.transform.parent = _places.Last().transform;
        resource.StartMove(_places.Last().transform);
    }

    public T Pull()
    {
        if (_resources.Count == 0)
            throw new InvalidOperationException();

        DestroyLastPlace();
        T lastResource = _resources.Last();
        _resources.Remove(lastResource);
        lastResource.transform.parent = null;
        return lastResource;
    }

    public bool CanPull() => _resources.Count > 0;

    private void CreateNewPlace()
    {
        PlaceInSlot place = Instantiate(_placeTemplate, transform);
        _places.Add(place);
    }

    private void ChangeLastPlacePosition(float resourceScaleY)
    {
        if (_places.Count > 0)
        {
            float offsetY = resourceScaleY * _scaleMultiplier + _distanceBetweenResources;
            float newPositionY = offsetY * (_places.Count - 1);
            _places.Last().transform.localPosition = new Vector3(0f, newPositionY, 0f);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }

    private void DestroyLastPlace()
    {
        if (_places.Count > 0)
        {
            var lastPlace = _places.Last();
            _places.Remove(lastPlace);
            Destroy(lastPlace.gameObject);
        }
        else
        {
            throw new InvalidOperationException();
        }
    }
}