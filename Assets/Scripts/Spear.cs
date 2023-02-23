using System;
using System.Collections;
using UnityEngine;

public class Spear : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _speed;
    [Min(0)]
    [SerializeField] private float _damage;
    [SerializeField] private ParticleSystem _trail;

    private Vector3 _direction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Animal animal))
            animal.Apply(_damage);
    }

    public void Throw(Transform target)
    {
        if (target == null)
            throw new ArgumentNullException(nameof(target));

        _trail.Play();
        StartCoroutine(Move(target));
    }

    private IEnumerator Move(Transform target)
    {
        while (transform.position != target.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
            _direction = target.position - transform.position;
            transform.rotation = Quaternion.LookRotation(_direction);
            yield return null;
        }

        transform.parent = target;
    }
}