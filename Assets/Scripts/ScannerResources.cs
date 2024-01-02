using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScannerResources : MonoBehaviour
{
    public event UnityAction<Resourse> ResourceDetect;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Resourse resourse) && resourse.Detected == false)
        {
            resourse.SetStatusDetect(true);
            ResourceDetect?.Invoke(resourse);
        }
    }
}
