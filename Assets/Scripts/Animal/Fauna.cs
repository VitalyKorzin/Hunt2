using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Fauna : MonoBehaviour
{
    [SerializeField] private List<Animal> _animals;

    public IReadOnlyList<Animal> Animals => _animals;

    public event UnityAction Died;

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

        foreach (Animal animal in _animals)
            animal.Died += OnAnimalDied;
    }

    private void OnDisable()
    {
        foreach (Animal animal in _animals)
            animal.Died -= OnAnimalDied;
    }

    private void OnAnimalDied(Animal animal)
    {
        animal.Died -= OnAnimalDied;
        _animals.Remove(animal);

        if (_animals.Count == 0)
            Died?.Invoke();
    }

    private void Validate()
    {
        if (_animals == null)
            throw new InvalidOperationException();

        if (_animals.Count == 0)
            throw new InvalidOperationException();

        foreach (Animal animal in _animals)
            if (animal == null)
                throw new InvalidOperationException();
    }
}