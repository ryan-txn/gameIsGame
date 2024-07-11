using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DetectPlayerEntry : MonoBehaviour
{
    private BoxCollider2D roomBoundsCollider;

    [SerializeField]
    private GameObject corridorBlocker;

    public bool playerEntered = false;

    private void Start()
    {
        // Assume room bounds collider is a larger collider representing the room's bounds
        roomBoundsCollider = GetComponent<BoxCollider2D>();
        if (roomBoundsCollider == null)
        {
            Debug.Log("no collider");
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
            Vector3 newSpawnpoint = spawnpoint + new Vector3(0, (height / 2) + 1, 0);
            Instantiate(corridorBlocker, newSpawnpoint, Quaternion.identity);
            newSpawnpoint = spawnpoint + new Vector3(0, -(height / 2) - 1, 0);
            Instantiate(corridorBlocker, newSpawnpoint, Quaternion.identity);
            newSpawnpoint = spawnpoint + new Vector3((width / 2) + 1, 0, 0);
            Instantiate(corridorBlocker, newSpawnpoint, Quaternion.identity);
            newSpawnpoint = spawnpoint + new Vector3(-(width / 2) - 1, 0, 0);
            Instantiate(corridorBlocker, newSpawnpoint, Quaternion.identity);
            roomBoundsCollider.enabled = false;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Player exited a room");
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
