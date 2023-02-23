using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public abstract class PurchaseZone : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _totalPrice;
    [Min(0)]
    [SerializeField] private int _increasingPrice;
    [Min(0)]
    [SerializeField] private float _secondsBetweenPayment;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Transform _paymentPoint;

    private Coroutine _acceptingPaymentJob;
    private int _remainingPrice;

    public int TotalPrice => _totalPrice;
    public int RemainingPrice => _remainingPrice;

    public event UnityAction Purchased;
    public event UnityAction PaymentAccepted;

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
    {
        _remainingPrice = _totalPrice;
        ShowRemainingPrice();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Inventory inventory))
            _acceptingPaymentJob = StartCoroutine(AcceptPayment(inventory));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Inventory _))
        {
            if (_acceptingPaymentJob != null)
                StopCoroutine(_acceptingPaymentJob);
        }
    }

    protected abstract bool CanPull(Inventory inventory);

    protected abstract Resource PullResource(Inventory inventory);

    private IEnumerator AcceptPayment(Inventory inventory)
    {
        var delay = new WaitForSeconds(_secondsBetweenPayment);

        while (_remainingPrice > 0 && CanPull(inventory))
        {
            yield return delay;
            _remainingPrice--;
            ShowRemainingPrice();
            Resource resource = PullResource(inventory);
            resource.ReachedTargetPosition += OnReachedTargetPosition;
            resource.StartMove(_paymentPoint);
            PaymentAccepted?.Invoke();
        }

        if (_remainingPrice <= 0)
        {
            Purchased?.Invoke();
            _remainingPrice = _totalPrice + _increasingPrice;
            ShowRemainingPrice();
        }
    }

    private void OnReachedTargetPosition(Resource resource)
    {
        resource.ReachedTargetPosition += OnReachedTargetPosition;
        Destroy(resource.gameObject);
    }

    private void ShowRemainingPrice() 
        => _text.text = _remainingPrice.ToString();

    private void Validate()
    {
        if (_text == null)
            throw new InvalidOperationException();

        if (_paymentPoint == null)
            throw new InvalidOperationException();
    }
}