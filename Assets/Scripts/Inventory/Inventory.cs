using System;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private float _distanceBetweenSlots;
    [Min(0)]
    [SerializeField] private float _scaleMultiplier;
    [SerializeField] private MeatsSlot _meatsSlot;
    [SerializeField] private BonesSlot _bonesSlot;

    public event UnityAction<int> MeatsCountChanged;
    public event UnityAction<int> BonesCountChanged;

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

    public void Push(Resource resource)
    {
        if (resource == null)
            throw new ArgumentNullException(nameof(resource));

        Push((dynamic)resource);
    }

    public Bone PullBone()
    {
        Bone bone = Pull(_bonesSlot);
        BonesCountChanged?.Invoke(_bonesSlot.Count);
        return bone;
    }

    public Meat PullMeat()
    {
        Meat meat = Pull(_meatsSlot);
        _bonesSlot.transform.localPosition -= GetOffset(meat.transform.lossyScale.y);
        MeatsCountChanged?.Invoke(_meatsSlot.Count);
        return meat;
    }

    public bool CanPullMeat() => CanPull(_meatsSlot);

    public bool CanPullBone() => CanPull(_bonesSlot);

    private bool CanPull<T>(Slot<T> slot) where T : Resource
        => slot.CanPull();

    private T Pull<T>(Slot<T> slot) where T: Resource
    {
        if (slot.CanPull())
            return slot.Pull();
        else
            throw new InvalidOperationException();
    }

    private void Push(Meat meat)
    {
        _meatsSlot.Push(meat);
        _bonesSlot.transform.localPosition += GetOffset(meat.transform.lossyScale.y);
        MeatsCountChanged?.Invoke(_meatsSlot.Count);
    }

    private void Push(Bone bone)
    {
        _bonesSlot.Push(bone);
        BonesCountChanged?.Invoke(_bonesSlot.Count);
    }

    private Vector3 GetOffset(float resourceScaleY)
    {
        float offsetY = resourceScaleY * _scaleMultiplier + _distanceBetweenSlots;
        return new Vector3(0f, offsetY, 0f);
    }

    private void Validate()
    {
        if (_meatsSlot == null)
            throw new InvalidOperationException();

        if (_bonesSlot == null)
            throw new InvalidOperationException();
    }
}