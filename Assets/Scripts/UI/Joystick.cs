using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;
    [SerializeField] private Canvas _canvas;

    private readonly float _maximumMagnitude = 1f;
    private readonly float _moveThreshold = 0.9f;

    private RectTransform _rectTransform;
    private Vector2 _difference;
    private Vector2 _position;
    private Vector2 _radius;
    private Camera _camera;

    public Vector2 Input { get; private set; }
    public bool Used => Input != Vector2.zero;

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
    
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _radius = _background.sizeDelta / 2;
        _camera = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _position = RectTransformUtility.WorldToScreenPoint(_camera, _background.position);
        Input = (eventData.position - _position) / (_radius * _canvas.scaleFactor);
        Vector2.ClampMagnitude(Input, _maximumMagnitude);

        if (Input.magnitude > _moveThreshold)
        {
            _difference = Input * (Input.magnitude - _moveThreshold) * _radius;
            _background.anchoredPosition += _difference;
        }

        _handle.anchoredPosition = Input * _radius;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _background.anchoredPosition = GetAnchoredPosition(eventData.position);
        _background.gameObject.SetActive(true);
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _background.gameObject.SetActive(false);
        Input = Vector2.zero;
        _handle.anchoredPosition = Vector2.zero;
    }

    private Vector2 GetAnchoredPosition(Vector2 screenPosition)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPosition, _camera, out Vector2 localPoint))
        {
            Vector2 pivotOffset = _rectTransform.pivot * _rectTransform.sizeDelta;
            return localPoint - (_background.anchorMax * _rectTransform.sizeDelta) + pivotOffset;
        }

        return Vector2.zero;
    }

    private void Validate()
    {
        if (_background == null)
            throw new InvalidOperationException();

        if (_handle == null)
            throw new InvalidOperationException();

        if (_canvas == null)
            throw new InvalidOperationException();
    }
}