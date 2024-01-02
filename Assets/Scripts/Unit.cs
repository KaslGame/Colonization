using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform _pocketTransform;
    [SerializeField] private float _speed;

    public bool IsBusy => _isBusy;
    public bool IsBusyCreateBase => _isBusyCreateBase;

    private Transform _baseTransform;
    private Transform _targetPosition;
    private Resourse _selectResource;

    private bool _isBusy;
    private bool _isBusyCreateBase;

    private void Update()
    {
        if (_isBusy && _targetPosition != null)
        {
            Vector3 newTargetPositon = new Vector3(_targetPosition.transform.position.x, transform.position.y, _targetPosition.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, newTargetPositon, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resourse resource) && resource == _selectResource && _isBusy)
        {
            resource.Take(_pocketTransform);
            SetTask(_baseTransform, false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Base baseCollider) && _isBusy && _isBusyCreateBase == false)
        {
            WareHouse wareHouse = baseCollider.GetComponent<WareHouse>();

            CompleteCollectTask(wareHouse);
            baseCollider.AddUnit(this);
        }
        else if (collision.gameObject.TryGetComponent(out Base newBase) && _isBusy && _isBusyCreateBase)
        {
            Base oldBase = _baseTransform.GetComponent<Base>();

            oldBase.DestroyFlag();
            CompleteCreateBaseTask();
            newBase.AddUnit(this);
        }
    }

    public void SetTask(Resourse resource, Transform baseTransform)
    {
        _selectResource = resource;
        _baseTransform = baseTransform;
        _targetPosition = _selectResource.transform;
        _isBusy = true;
    }

    public void SetTask(Transform targetTransform, bool isBusyCreateBase)
    {
        _targetPosition = targetTransform;
        _isBusy = true;
        _isBusyCreateBase = isBusyCreateBase;
    }
    
    private void CompleteCollectTask(WareHouse wareHouse)
    {
        _targetPosition = transform;
        _isBusy = false;
        wareHouse.AddResource(_selectResource.RewardResource);
        _selectResource.Place();
    }

    private void CompleteCreateBaseTask()
    {
        _targetPosition = transform;
        _baseTransform = null;
        _isBusy = false;
        _isBusyCreateBase = false;
    }
}
