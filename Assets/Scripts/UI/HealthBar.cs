using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Animal _animal;
    [SerializeField] private Slider _basicFill;
    [SerializeField] private Slider _rearFill;
    [SerializeField] private Image _rearFillImage;

    private readonly float _fadingEndValue = 0f;
    private readonly float _fadingDuration = 0.7f;
    private readonly float _appearanceEndValue = 1f;
    private readonly float _appearanceDuration = 0f;

    private Camera _camera;

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

        _animal.HealthChanged += OnHealthChanged;
        _animal.Died += OnDied;
    }

    private void OnDisable()
    {
        _animal.HealthChanged -= OnHealthChanged;
        _animal.Died -= OnDied;
    }

    private void Start() => _camera = Camera.main;

    private void Update() => LookAtCamera();

    private void LookAtCamera()
    {
        Vector3 worldPosition = transform.position + _camera.transform.rotation * Vector3.forward;
        Vector3 worldUp = _camera.transform.rotation * Vector3.up;
        transform.LookAt(worldPosition, worldUp);
    }

    private void OnHealthChanged(float currentHealth, float maximumHealth)
    {
        _rearFillImage.DOFade(_appearanceEndValue, _appearanceDuration);
        _rearFill.value = _basicFill.value;
        _basicFill.value = currentHealth / maximumHealth;
        _rearFillImage.DOFade(_fadingEndValue, _fadingDuration);
    }

    private void OnDied(Animal animal) => Destroy(gameObject);

    private void Validate()
    {
        if (_animal == null)
            throw new InvalidOperationException();

        if (_basicFill == null)
            throw new InvalidOperationException();

        if (_rearFill == null)
            throw new InvalidOperationException();

        if (_rearFillImage == null)
            throw new InvalidOperationException();
    }
}