using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HeroAttacker : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _secondsBeforeThrowing;
    [Min(0)]
    [SerializeField] private float _secondsBeforeSwing;
    [SerializeField] private Spear _template;
    [SerializeField] private Transform _spearPosition;
    [SerializeField] private Animator _animator;

    private Coroutine _attackingJob;
    private Spear _currentSpear;

    public event UnityAction<Animal> AttackBegun;
    public event UnityAction AttackIsOver;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animal animal) && animal.Alive)
        {
            if (_attackingJob != null)
                StopCoroutine(_attackingJob);

            animal.Died += OnAnimalDied;
            _animator.SetBool(HeroAnimator.Params.IsAttacking, true);
            AttackBegun?.Invoke(animal);
            _attackingJob = StartCoroutine(Attack(animal));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Animal animal))
        {
            if (_attackingJob != null)
                StopAttack(animal);
        }
    }

    private IEnumerator Attack(Animal target)
    {
        var delayBeforeThrowing = new WaitForSeconds(_secondsBeforeThrowing);
        var delayBeforeSwing = new WaitForSeconds(_secondsBeforeSwing);

        while (target != null)
        {
            CreateSpear();
            yield return delayBeforeThrowing;
            _currentSpear.transform.parent = null;
            _currentSpear.Throw(target.SpearTarget);
            yield return delayBeforeSwing;
        }

        StopAttack(target);
    }

    private void OnAnimalDied(Animal animal) => StopAttack(animal);

    private void StopAttack(Animal animal)
    {
        animal.Died -= OnAnimalDied;
        _animator.SetBool(HeroAnimator.Params.IsAttacking, false);
        AttackIsOver?.Invoke();
        StopCoroutine(_attackingJob);
        CreateSpear();
    }

    private void CreateSpear()
    {
        if (_currentSpear != null)
            Destroy(_currentSpear.gameObject);

        _currentSpear = Instantiate(_template, _spearPosition.position, _spearPosition.rotation);
        _currentSpear.transform.parent = _spearPosition;
    }

    private void Validate()
    {
        if (_template == null)
            throw new InvalidOperationException();

        if (_animator == null)
            throw new InvalidOperationException();

        if (_spearPosition == null)
            throw new InvalidOperationException();
    }
}