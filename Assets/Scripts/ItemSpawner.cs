using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private Resourse[] _itemPrefabs;
    [SerializeField] private float _timeBeetwenSpawn;

    private List<Transform> _spawnPoints;

    private void Start()
    {
        _spawnPoints = new List<Transform>(transform.childCount);

        foreach (Transform spawPoint in transform)
            _spawnPoints.Add(spawPoint);

        Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Length)], _spawnPoints[Random.Range(0, _spawnPoints.Count)].position, Quaternion.identity);
        StartCoroutine(RandomSpawnTimer(_timeBeetwenSpawn));
    }

    private IEnumerator RandomSpawnTimer(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            Instantiate(_itemPrefabs[Random.Range(0, _itemPrefabs.Length)], _spawnPoints[Random.Range(0, _spawnPoints.Count)].position, Quaternion.identity);
        }
    }
}
