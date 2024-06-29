using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _enemyList;

    [SerializeField]
    private float _minimumSpawnTime;

    [SerializeField]
    private float _maximumSpawnTime;

    private float _timeUntilSpawn;
    private GameObject _enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SetTimeUntilSpawn();  
        RandomiseEnemy(); 
    }

    // Update is called once per frame
    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime; //will reduce time until spawn by the amt of time that has passed this frame

        if (_timeUntilSpawn <= 0)
        {
            Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minimumSpawnTime, _maximumSpawnTime);
    }

    private void RandomiseEnemy()
    {
        int index = Random.Range(0, _enemyList.Count);
        _enemyPrefab = _enemyList[index];
    }
}
