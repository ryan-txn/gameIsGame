using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;

public class DetectPlayerEntry : MonoBehaviour
{
    private BoxCollider2D roomBoundsCollider;

    [SerializeField]
    private GameObject horCorridorBlocker;

    [SerializeField]
    private GameObject vertCorridorBlocker;

    [SerializeField]
    private string _doorsOpenedString;

    private InfoMessageUI _infoMessageUI;

    [HideInInspector]
    public bool playerEntered = false;

    [HideInInspector]
    public bool playerExited = false;

    [SerializeField]
    private float duration;

    GameObject corrBlocker1;
    GameObject corrBlocker2;
    GameObject corrBlocker3;
    GameObject corrBlocker4;

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
            EnemyCounter.SetEnemies(10);
        }

    }

    private void Update()
    {
        if (playerEntered && EnemyCounter.GetEnemyCount() == 0) //checking for player killed all enemies
        {
            Destroy(corrBlocker1);
            Destroy(corrBlocker2);
            Destroy(corrBlocker3);
            Destroy(corrBlocker4);
            Debug.Log("doors opened");
            playerEntered = false;
            if (_infoMessageUI != null)
            {
                _infoMessageUI.UpdateMessage(_doorsOpenedString);
            }
            StartCoroutine(ClearMessageAfterTime(duration));
        }
    }

    private IEnumerator ClearMessageAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (_infoMessageUI != null)
        {
            _infoMessageUI.ClearMessage();
            Debug.Log(duration + " seconds passed since leaving");
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
}
