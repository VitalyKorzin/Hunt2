using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HuntersSquad : MonoBehaviour
{
    [Min(0)]
    [SerializeField] private int _startAccessiblePlaceCount;
    [Min(0)]
    [SerializeField] private int _increasingPlacesCount;
    [Min(0)]
    [SerializeField] private int _level;
    [SerializeField] private SquadUpgradeZone _upgradeZone;
    [SerializeField] private HuntStartAnnunciator _annunciator;
    [SerializeField] private Fauna _fauna;
    [SerializeField] private List<PlaceInSquad> _places;

    private readonly List<PlaceInSquad> _accessiblePlaces = new List<PlaceInSquad>();
    private readonly List<Hunter> _hunters = new List<Hunter>();

    public event UnityAction Upgraded;
    public event UnityAction<int> LevelChanged;
    public event UnityAction<int> AccessiblePlacesCountChanged;

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

        _upgradeZone.Purchased += OnPurchased;
        _annunciator.HuntStarted += OnHuntStarted;
    }

    private void OnDisable()
    {
        _upgradeZone.Purchased -= OnPurchased;
        _annunciator.HuntStarted -= OnHuntStarted;
    }

    private void Start()
    {
        AddAccessiblePlaces(_accessiblePlaces.Count, _startAccessiblePlaceCount);
        LevelChanged?.Invoke(_level);
        Upgraded?.Invoke();
    }

    public void Add(Hunter hunter)
    {
        if (hunter == null)
            throw new ArgumentNullException(nameof(hunter));

        _hunters.Add(hunter);
    }

    public PlaceInSquad OccupyAccessiblePlace()
    {
        PlaceInSquad freePlace = GetFreePlace();

        if (freePlace == null)
            throw new InvalidOperationException();

        freePlace.Occupy();
        return freePlace;
    }

    public bool CanOccupyAccessiblePlace() 
        => GetFreePlace() != null;

    public int GetOccupiedPlacesCount() 
        => _accessiblePlaces.Where(place => place.Occupied).Count();

    private PlaceInSquad GetFreePlace() 
        => _accessiblePlaces.FirstOrDefault(place => place.Free);

    private void AddAccessiblePlaces(int startIndex, int count)
    {
        if (startIndex > _places.Count - 1)
            throw new InvalidOperationException();

        int totalCount = count + startIndex;

        if (totalCount > _places.Count)
            throw new InvalidOperationException();

        for (var i = startIndex; i < totalCount; i++)
        {
            _places[i].gameObject.SetActive(true);
            _accessiblePlaces.Add(_places[i]);
        }

        AccessiblePlacesCountChanged?.Invoke(_accessiblePlaces.Count);
    }

    private void OnPurchased()
    {
        AddAccessiblePlaces(_accessiblePlaces.Count, _increasingPlacesCount);
        _level++;
        LevelChanged?.Invoke(_level);
        Upgraded?.Invoke();
    }

    private void OnHuntStarted()
    {
        foreach (Hunter hunter in _hunters)
            hunter.StartHunt(_fauna);
    }

    private void Validate()
    {
        if (_upgradeZone == null)
            throw new InvalidOperationException();

        if (_places == null)
            throw new InvalidOperationException();

        if (_fauna == null)
            throw new InvalidOperationException();

        if (_annunciator == null)
            throw new InvalidOperationException();

        if (_places.Count == 0)
            throw new InvalidOperationException();

        foreach (PlaceInSquad place in _places)
            if (place == null)
                throw new InvalidOperationException();
    }
}