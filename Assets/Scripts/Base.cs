using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(WareHouse))]
[RequireComponent(typeof(ScannerResources))]
public class Base : MonoBehaviour
{
    [SerializeField] private List<Unit> _activeUnits = new List<Unit>();
    [SerializeField] private List<Unit> _passiveUnits = new List<Unit>();
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private int _countStartUnit = 0;

    private ScannerResources _scannerResources;
    private WareHouse _wareHouse;
    private FlagCreator _flagCreator;

    private List<Resourse> _findedResourses = new List<Resourse>();
    private List<Transform> _spawnPoints;
    private Transform _flagTransform;

    private int _maxUnits;
    private bool _isFlagCreated;
    private bool _isBaseSendCreateBaseUnit;

    public bool IsBaseSendCreateBaseUnit => _isBaseSendCreateBaseUnit;

    private void Awake()
    {
        _scannerResources = GetComponent<ScannerResources>();
        _wareHouse = GetComponent<WareHouse>();
        _flagCreator = GetComponent<FlagCreator>();

        _spawnPoints = new List<Transform>(transform.childCount);
        _maxUnits = transform.childCount;

        foreach (Transform spawPoint in transform)
            _spawnPoints.Add(spawPoint);
    }

    private void Start()
    {
        CreateNewUnits(_countStartUnit);
    }

    private void OnEnable()
    {
        _scannerResources.ResourceDetect += OnResourceDetect;
        _flagCreator.FlagCreated += OnFlagCreated;
    }

    private void OnDisable()
    {
        _scannerResources.ResourceDetect -= OnResourceDetect;
        _flagCreator.FlagCreated -= OnFlagCreated;
    }

    private void Update()
    {
        TrySendCreateBaseUnit();
        TryCreateNewUnit();
        TrySendCollectUnit();
    }

    private void OnFlagCreated(Transform flagTransform)
    {
        _flagTransform = flagTransform;
        _isFlagCreated = true;
    }

    private void OnResourceDetect(Resourse resourse)
    {
        if (_findedResourses.Contains(resourse) == false)
            _findedResourses.Add(resourse);
    }

    public void AddUnit(Unit unit)
    {
        if (_activeUnits.Count < _maxUnits)
        {
            _activeUnits.Add(unit);

            if (_passiveUnits.Contains(unit) == false)
                _passiveUnits.Add(unit);
        }
        else
        {
            Destroy(unit.gameObject);
        }

        SpawnUnits();
    }

    public void DestroyFlag()
    {
        _isFlagCreated = false;
        Destroy(_flagTransform.gameObject);
    }

    private void TryCreateNewUnit()
    {
        int costCreateUnit = 3;
        int oneUnit = 1;

        if (_passiveUnits.Count > 1 && _isFlagCreated)
        {
            return;
        }
        else if (_wareHouse.Resources >= costCreateUnit && _activeUnits.Count < _maxUnits)
        {
            CreateNewUnits(oneUnit);
            _wareHouse.TakeResources(costCreateUnit);
        }
    }

    private void TrySendCreateBaseUnit()
    {
        int costCreateBase = 5;

        if (_passiveUnits.Count > 1 && _wareHouse.Resources >= costCreateBase && _isFlagCreated)
        {
            Unit removedUnit = GetLastUnit();

            _wareHouse.TakeResources(costCreateBase);
            removedUnit.SetTask(_flagTransform, true);
            _passiveUnits.Remove(removedUnit);
        }
    }

    private void TrySendCollectUnit()
    {
        if (CheckEnoughUnitsResourses())
            GetLastUnit().SetTask(GetLastResource(), transform);
    }

    private void SpawnUnits()
    {
        for (int i = 0; i < _activeUnits.Count; i++)
            _activeUnits[i].transform.position = _spawnPoints[i].transform.position;
    }

    private void CreateNewUnits(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Unit newUnit = Instantiate(_unitPrefab, transform.position, Quaternion.identity);
            AddUnit(newUnit);
        }
    }

    private Resourse GetLastResource()
    {
        Resourse resourse = _findedResourses[_findedResourses.Count - 1];
        _findedResourses.Remove(resourse);
        return resourse;
    }

    private Unit GetLastUnit()
    {
        Unit unit = _activeUnits[_activeUnits.Count - 1];
        _activeUnits.Remove(unit);
        return unit;
    }

    private bool CheckEnoughUnitsResourses()
    {
        return _activeUnits.Count > 0 && _findedResourses.Count > 0;
    }
}
