using System;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class ResourcesCollector : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _radius;
    [SerializeField] private Inventory _inventory;

    private void OnEnable()
    {
        if (_inventory == null)
        {
            enabled = false;
            throw new InvalidOperationException();
        }
    }

    private void Start() 
        => GetComponent<CapsuleCollider>().radius = _radius;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Resource resource))
            _inventory.Push(resource);
    }
}