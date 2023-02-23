using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator), typeof(AnimalMover))]
public class Animal : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _health;
    [SerializeField] private Transform _spearTarget;

    private readonly float _delayBeforeFalling = 1.5f;
    private readonly float _fallDuration = 2f;
    private readonly float _fallingSpeed = 1f;

    private AnimalMover _mover;
    private Animator _animator;
    private float _maximumHealth;

    public bool Alive => _health > 0;
    public Transform SpearTarget => _spearTarget;

    public event UnityAction<float, float> HealthChanged;
    public event UnityAction<Animal> Died;

    private void OnEnable()
    {
        if (_spearTarget == null)
        {
            enabled = false;
            throw new InvalidOperationException();
        }
    }

    private void Start()
    {
        _mover = GetComponent<AnimalMover>();
        _animator = GetComponent<Animator>();
        _maximumHealth = _health;
    }

    public void Apply(float damage)
    {
        if (_health <= 0)
            return;

        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage));

        _health -= damage;
        HealthChanged?.Invoke(_health, _maximumHealth);

        if (_health <= 0)
            StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        _animator.SetTrigger(AnimalAnimator.Params.Dead);
        _mover.StopWalk();
        yield return new WaitForSeconds(_delayBeforeFalling);
        Died?.Invoke(this);
        StartCoroutine(Fall());
    }

    private IEnumerator Fall()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _fallDuration)
        {
            transform.Translate(_fallingSpeed * Time.deltaTime * Vector3.down);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}