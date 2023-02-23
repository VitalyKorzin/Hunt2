using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDispenser : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _startedBonesCount;
    [Min(0)]
    [SerializeField] private int _startedMeatsCount;
    [SerializeField] private List<Resource> _templates;
    [SerializeField] private Inventory _inventory;

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

    private void Start() => DispenseToHero();

    public List<Resource> Dispense(Vector3 position, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        var resources = new List<Resource>();

        for (var i = 0; i < count; i++)
        {
            var template = _templates[UnityEngine.Random.Range(0, _templates.Count)];
            resources.Add(Instantiate(template, position, Quaternion.identity));
        }

        return resources;
    }

    private void DispenseToHero()
    {
        DispenseToHero<Bone>(_startedBonesCount);
        DispenseToHero<Meat>(_startedMeatsCount);
    }

    private void DispenseToHero<T>(int count) where T: Resource
    {
        for (var i = 0; i < count; i++)
        {
            Resource resource = _templates.FirstOrDefault(r => r is T);
            Instantiate(resource, _inventory.transform.position, Quaternion.identity);
        }
    }

    private void Validate()
    {
        if (_inventory == null)
            throw new InvalidOperationException();

        if (_templates == null)
            throw new InvalidOperationException();

        if (_templates.Count == 0)
            throw new InvalidOperationException();

        foreach (Resource template in _templates)
            if (template == null)
                throw new InvalidOperationException();
    }
}