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

    [SerializeField]
    private int _maxSpawnCount;

    private int _spawnCount = 0;

    private DetectPlayerEntry detectPlayerEntry;

    // Start is called before the first frame update
    void Start()
    {
        SetTimeUntilSpawn();  
        RandomiseEnemy();
        GameObject enemyRoom = transform.parent.gameObject;

        // Access a script/component on the parent GameObject
        detectPlayerEntry = enemyRoom.GetComponent<DetectPlayerEntry>();
    }

    // Update is called once per frame
    void Update()
    {
        if (detectPlayerEntry.playerEntered)
        {
            _timeUntilSpawn -= Time.deltaTime; //will reduce time until spawn by the amt of time that has passed this frame

            if (_timeUntilSpawn <= 0 && _spawnCount < _maxSpawnCount)
            {
                Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
                SetTimeUntilSpawn();
                _spawnCount++;
            }
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
