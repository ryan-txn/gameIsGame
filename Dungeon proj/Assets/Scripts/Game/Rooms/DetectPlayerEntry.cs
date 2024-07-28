using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Controls;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;

public class DetectPlayerEntry : MonoBehaviour
{
    private BoxCollider2D roomBoundsCollider;

    [SerializeField]
    private GameObject horCorridorBlocker;

    [SerializeField]
    private GameObject vertCorridorBlocker;

    [SerializeField]
    private string _doorsOpenedString;

    [SerializeField]
    private int _enemyCount = 0;

    private InfoMessageUI _infoMessageUI;

    [HideInInspector]
    public bool playerEntered = false;

    [HideInInspector]
    public bool playerExited = false;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float fade_duration;

    private GameObject corrBlocker1;
    private GameObject corrBlocker2;
    private GameObject corrBlocker3;
    private GameObject corrBlocker4;

    [SerializeField]
    private LayerMask _enemyLayerMask;

    public UnityEvent EnterBossRoom;
    public UnityEvent BossRoomClear;

    private void Start()
    {
        // room bounds collider is a larger collider representing the room's bounds
        roomBoundsCollider = GetComponent<BoxCollider2D>();
        if (roomBoundsCollider == null)
        {
            Debug.Log("no collider");
        }

        _infoMessageUI = GameObject.Find("Info message").GetComponent<InfoMessageUI>();
        if (_infoMessageUI == null)
        {
            Debug.LogError("no info message component");
        }

        EnemySpawner[] enemySpawners = GetComponentsInChildren<EnemySpawner>();
        foreach (EnemySpawner spawner in enemySpawners)
        {
            _enemyCount += spawner.GetMaxSpawnCount();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player entered a room");
            playerEntered = true;
            Tilemap roomWalls = FindTilemapWithTag(gameObject, "Wall");
            roomWalls.CompressBounds();
            int height = roomWalls.size.y;
            int width = roomWalls.size.x;
            Vector3 spawnpoint = transform.position;
            Vector3 newSpawnpoint = spawnpoint + new Vector3(0, (height / 2) - 1, 0);
            corrBlocker1 = Instantiate(horCorridorBlocker, newSpawnpoint, Quaternion.identity);
            newSpawnpoint = spawnpoint + new Vector3(0, -(height / 2), 0);
            corrBlocker2 = Instantiate(horCorridorBlocker, newSpawnpoint, Quaternion.identity);
            newSpawnpoint = spawnpoint + new Vector3((width / 2) - 1, 0, 0);
            corrBlocker3 = Instantiate(vertCorridorBlocker, newSpawnpoint, Quaternion.identity);
            newSpawnpoint = spawnpoint + new Vector3(-(width / 2), 0, 0);
            corrBlocker4 = Instantiate(vertCorridorBlocker, newSpawnpoint, Quaternion.identity);

            roomBoundsCollider.enabled = false; //delete collider
            EnemyCounter.SetEnemies(_enemyCount);

            Debug.Log("DetectPlayerEntry: enemy counter is " + _enemyCount);

            FindObjectOfType<AudioManager>().PlaySFX("Door close sfx");

            EnterBossRoom.Invoke(); //Boss room event
        }

    }

    private void Update()
    {
        if (playerEntered && EnemyCounter.GetEnemyCount() <= 0) //checking for player killed all enemies
        {
            Destroy(corrBlocker1);
            Destroy(corrBlocker2);
            Destroy(corrBlocker3);
            Destroy(corrBlocker4);
            Debug.Log("doors opened");
            playerEntered = false;
            if (_infoMessageUI != null)
            {
                _infoMessageUI.UpdateMessage(_doorsOpenedString, fade_duration, duration, fade_duration);
            }
            else
            {
                Debug.LogError("info message not assigned");
            }

            BossRoomClear.Invoke(); //Boss room event
        }
    }

    Tilemap FindTilemapWithTag(GameObject parent, string tag)
    {
        Tilemap[] tilemaps = parent.GetComponentsInChildren<Tilemap>();
        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap.CompareTag(tag))
            {
                return tilemap;
            }
        }
        Debug.LogError("no wall tilemap found");
        return null;
    }

    public void DeathExplosion()
    {
        Debug.Log("Death explosion complete");
        float _explosionRadius = 50.0f;

        //Hit enemies in the set explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collider detected: " + collider.gameObject.name);
            var enemy = collider.GetComponent<EnemyMovement>();

            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            HealthController healthController = collider.GetComponent<HealthController>();

            if (healthController != null && rb != null && enemy != null)
            {
                healthController.TakeDamage(50f);
            }
        }
    }

    public void SpawnPortal()
    {
        SpawnPortal spawnPortal = GetComponent<SpawnPortal>();
        if (spawnPortal != null)
        {
            GameObject Portal = spawnPortal.Spawn();
            SceneTransition sceneTransition = Portal.GetComponent<SceneTransition>();
            sceneTransition.UpdateSceneToLoadString();
        }
        else
        {
            Debug.Log("Spawn Portal not found - either not boss room or error");
        }
    }
}
