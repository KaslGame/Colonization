using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagBase : MonoBehaviour
{
    [SerializeField] private Base _basePrefab;

    private float yOffset = 1;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Unit unit))
        {
            if (unit.IsBusyCreateBase == true)
            {
                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);

                Instantiate(_basePrefab, spawnPosition, Quaternion.identity);
            }
        }
    }
}
