using UnityEngine;
using System.Collections;

public class LuchaSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _luchaPrefab;

    [SerializeField]
    private Vector3 _spawnAreaMin;

    [SerializeField]
    private Vector3 _spawnAreaMax;

    [SerializeField]
    private int _maxLuchasInScene;

    [SerializeField]
    private float _spawnInterval = 1.0f; // Intervalo de tempo entre spawns

    private void Start()
    {
        StartCoroutine(SpawnLuchasOverTime());
    }

    private IEnumerator SpawnLuchasOverTime()
    {
        while (true)
        {
            int currentCoinCount = GameObject.FindGameObjectsWithTag("Luchador").Length;

            if (currentCoinCount < _maxLuchasInScene)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(_spawnAreaMin.x, _spawnAreaMax.x),
                    Random.Range(_spawnAreaMin.y, _spawnAreaMax.y),
                    Random.Range(_spawnAreaMin.z, _spawnAreaMax.z)
                );

                Instantiate(_luchaPrefab, randomPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(_spawnInterval);
        }
    }
}