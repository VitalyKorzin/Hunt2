using UnityEngine;

public class PlaceInSquad : MonoBehaviour
{
    [SerializeField] private bool _occupied;

    public bool Free => _occupied == false;
    public bool Occupied => _occupied;

    public void Occupy() => _occupied = true;
}