using System;
using UnityEngine;

[RequireComponent(typeof(HunterMover), typeof(HunterAttacker))]
public class Hunter : MonoBehaviour
{
    private Fauna _fauna;
    private HunterMover _mover;
    private HunterAttacker _attacker;

    private void OnDisable() 
        => _attacker.TargetDied -= OnTargetDied;

    private void Awake()
    {
        _mover = GetComponent<HunterMover>();
        _attacker = GetComponent<HunterAttacker>();
        _attacker.TargetDied += OnTargetDied;
    }

    public void Initialize(Transform target) 
        => _mover.StartMove(target);

    public void StartHunt(Fauna fauna)
    {
        if (fauna == null)
            throw new ArgumentNullException(nameof(fauna));

        _fauna = fauna;
        MoveToAnimal();
    }

    private void OnTargetDied() => MoveToAnimal();

    private void MoveToAnimal()
    {
        if (_fauna.Animals.Count > 0)
            _mover.StartMove(_fauna.Animals[0]);
    }
}