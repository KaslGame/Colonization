using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WareHouse : MonoBehaviour
{
    private int _resources;

    public event UnityAction<int> ResourceAmountChanged;
    public int Resources => _resources;

    public void AddResource(int amount)
    {
        _resources += amount;
        ResourceAmountChanged?.Invoke(_resources);
    }

    public void TakeResources(int amount)
    {
        _resources -= amount;
        ResourceAmountChanged?.Invoke(_resources);
    }
}
