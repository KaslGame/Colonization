using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Base))]
[RequireComponent(typeof(TrackerClick))]
public class FlagCreator : MonoBehaviour
{
    [SerializeField] private GameObject _flagPrefab;

    public event UnityAction<Transform> FlagCreated;

    private TrackerClick _trackerClick;
    private Base _base;
    private GameObject _spawnedFlag;
    private bool _isReadySetFlag = false;
    private bool _isFlagCreated;

    private void Awake()
    {
        _trackerClick = GetComponent<TrackerClick>();
        _base = GetComponent<Base>();
    }

    private void OnEnable()
    {
        _trackerClick.BaseSelect += OnBaseSelect;
    }

    private void OnDisable()
    {
        _trackerClick.BaseSelect -= OnBaseSelect;
    }

    private void Update()
    {
        TrySpawnFlag();
    }

    private void OnBaseSelect(bool baseSelect)
    {
        _isReadySetFlag = baseSelect;
    }

    private void TrySpawnFlag()
    {
        if (_isReadySetFlag && _base.IsBaseSendCreateBaseUnit == false)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent(out Ground _))
                    {
                        if (_isFlagCreated)
                        {
                            Destroy(_spawnedFlag);
                            SpawnFlag(hit.point);
                        }
                        else
                        {
                            SpawnFlag(hit.point);
                        }

                        FlagCreated?.Invoke(_spawnedFlag.transform);
                    }
                }
            }
        }
    }

    private void SpawnFlag(Vector3 spawnPosition)
    {
        float yOffset = 1f;
        Vector3 newSpawnPosition = new Vector3(spawnPosition.x, spawnPosition.y + yOffset, spawnPosition.z);
        _spawnedFlag = Instantiate(_flagPrefab, newSpawnPosition, Quaternion.identity);
        _isFlagCreated = true;
    }
}
