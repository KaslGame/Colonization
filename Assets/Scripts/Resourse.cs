using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resourse : MonoBehaviour
{
    [SerializeField] private int _rewardResource = 1;

    private bool _detected;

    public int RewardResource => _rewardResource;
    public bool Detected => _detected;

    public void Take(Transform pocketTransform)
    {
        transform.position = pocketTransform.position;
        transform.parent = pocketTransform;
    }

    public void SetStatusDetect(bool isDetect)
    {
        _detected = isDetect;
    }

    public void Place()
    {
        Destroy(this.gameObject);
    }
}
