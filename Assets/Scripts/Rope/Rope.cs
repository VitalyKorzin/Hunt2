using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Rope : MonoBehaviour
{
    [SerializeField] private Node[] _nodes;

    private LineRenderer _lineRenderer;

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
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _nodes.Length;
    }

    public void Initialize(Transform firstNodePosition, Transform lastNodePosition)
    {
        if (firstNodePosition == null)
            throw new ArgumentNullException(nameof(firstNodePosition));

        if (lastNodePosition == null)
            throw new ArgumentNullException(nameof(lastNodePosition));

        StartCoroutine(Draw(firstNodePosition, lastNodePosition));
    }

    private IEnumerator Draw(Transform firstNodePosition, Transform lastNodePosition)
    {
        Node firstNode = _nodes[0];
        Node lastNode = _nodes[_nodes.Length - 1];

        while (firstNodePosition != null && lastNodePosition != null)
        {
            firstNode.transform.position = firstNodePosition.position;
            lastNode.transform.position = lastNodePosition.position;

            for (var i = 0; i < _nodes.Length; i++)
                _lineRenderer.SetPosition(i, _nodes[i].transform.position);

            yield return null;
        }
    }

    private void Validate()
    {
        if (_nodes == null)
            throw new InvalidOperationException();

        if (_nodes.Length == 0)
            throw new InvalidOperationException();

        foreach (Node node in _nodes)
            if (node == null)
                throw new InvalidOperationException();
    }
}