using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HuntStartAnnunciator : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _delayBeforeStarted;
    [SerializeField] private HuntStartAnnunciatorDisplay _display;

    public event UnityAction HuntStarted;

    private void OnEnable()
    {
        if (_display == null)
        {
            enabled = false;
            throw new InvalidOperationException();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Hero _))
            StartCoroutine(Announce());
    }

    private IEnumerator Announce()
    {
        _display.Appear();
        yield return new WaitForSeconds(_delayBeforeStarted);
        _display.Fade();
        HuntStarted?.Invoke();
    }
}