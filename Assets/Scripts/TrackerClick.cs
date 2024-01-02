using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SphereCollider))]
public class TrackerClick : MonoBehaviour
{
    public event UnityAction<bool> BaseSelect;

    private bool _baseSelected = false;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    //private void OnMouseDown()
    //{
    //    Collider[] objectColliders = GetComponentsInChildren<Collider>();

    //    foreach (Collider collider in objectColliders)
    //        if (!collider.isTrigger)
    //            Debug.Log("Объект был нажат и он не триггер");
    //}

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            _sphereCollider.enabled = false;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.TryGetComponent(out Base _) && hit.collider.gameObject == gameObject)
                {
                    if (_baseSelected)
                        _baseSelected = false;
                    else
                        _baseSelected = true;

                    BaseSelect?.Invoke(_baseSelected);
                }
            }

            _sphereCollider.enabled = true;
        }
    }
}
