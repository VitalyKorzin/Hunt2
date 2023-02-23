using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class HeroMover : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _speed;
    [SerializeField] private Joystick _joystick;

    private Animator _animator;
    private Vector3 _duration;

    public Vector3 Duration => _duration;

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

    private void Start() 
        => _animator = GetComponent<Animator>();

    private void Update()
    {
        if (_joystick.Used)
            Move();

        _animator.SetFloat(HeroAnimator.Params.MovementMagnitude, _joystick.Input.magnitude);
    }

    private void Move()
    {
        _duration = new Vector3(_joystick.Input.x, 0f, _joystick.Input.y);
        transform.Translate(_speed * Time.deltaTime * _duration);
    }

    private void Validate()
    {
        if (_joystick == null)
            throw new InvalidOperationException();
    }
}