using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _coinPrefab;

    [SerializeField]
    private Vector3 _spawnAreaMin;

    [SerializeField]
    private Vector3 _spawnAreaMax;

    [SerializeField]
    private int _maxCoinsInScene;

    [SerializeField]
    private float _spawnInterval = 1.0f; 

    private void Start()
    {
        StartCoroutine(SpawnCoinsOverTime());
    }

    private IEnumerator SpawnCoinsOverTime()
    {
        while (true)
        {
            int currentCoinCount = GameObject.FindGameObjectsWithTag("Coin").Length;

            if (currentCoinCount < _maxCoinsInScene)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(_spawnAreaMin.x, _spawnAreaMax.x),
                    Random.Range(_spawnAreaMin.y, _spawnAreaMax.y),
                    Random.Range(_spawnAreaMin.z, _spawnAreaMax.z)
                );

                Instantiate(_coinPrefab, randomPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}