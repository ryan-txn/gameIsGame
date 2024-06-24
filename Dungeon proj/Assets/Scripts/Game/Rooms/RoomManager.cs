using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] roomPrefabs;

    [SerializeField]
    private Tilemap startRoomWalls;


    private void Awake()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        //spawn room from prefabs
        startRoomWalls.CompressBounds();
        int gridWidth = startRoomWalls.size.x;
        int gridHeight = startRoomWalls.size.y;
        Vector3 spawnPoint = new Vector3(0, 0, 0);
        int positionIndex = Random.Range(0, 4);

        var direction = Direction3D.GetRandomCardinalDirection(positionIndex);

        string wallTilemapTag = "Wall";
        Tilemap wallTilemap = FindTilemapWithTag(roomPrefabs[0], wallTilemapTag);
        wallTilemap.CompressBounds();
        int enmyGridWidth = wallTilemap.size.x;
        int enmyGridHeight = wallTilemap.size.y;
        Debug.Log(enmyGridHeight + ", " + enmyGridWidth);



        if (positionIndex == 0 || positionIndex == 1)
        {
            if (positionIndex == 0)
            {
                //delete walls
                Vector3Int topGap1 = new (-1, (gridHeight / 2) - 1, 0);
                Vector3Int topGap2 = new (0, (gridHeight / 2) - 1, 0);
                startRoomWalls.SetTile(topGap1, null);
                startRoomWalls.SetTile(topGap2, null);

                Vector3Int enmyBottomGap1 = new(-1, -(enmyGridHeight / 2), 0);
                Vector3Int enmyBottomGap2 = new(0, -(enmyGridHeight / 2), 0);
                Debug.Log(enmyBottomGap1 + ", " + enmyBottomGap2); // shld be -8
                wallTilemap.CompressBounds();
                wallTilemap.SetTile(enmyBottomGap1, null);
                wallTilemap.SetTile(enmyBottomGap2, null);
            } else {
                //delete walls
                Vector3Int bottomGap1 = new(-1, -(gridHeight / 2), 0);
                Vector3Int bottomGap2 = new(0, -(gridHeight / 2), 0);
                startRoomWalls.SetTile(bottomGap1, null);
                startRoomWalls.SetTile(bottomGap2, null);

                Vector3Int enmyTopGap1 = new(-1, (enmyGridHeight / 2) - 1, 0);
                Vector3Int enmyTopGap2 = new(0, (enmyGridHeight / 2) - 1, 0); 
                Debug.Log(enmyTopGap1 + ", " + enmyTopGap2); // shld be 7
                wallTilemap.CompressBounds();
                wallTilemap.SetTile(enmyTopGap1, null);
                wallTilemap.SetTile(enmyTopGap2, null);
            }

            for (int i = 0; i < 13; i++)
            {
                spawnPoint += direction;
            }
            Instantiate(roomPrefabs[0], spawnPoint, Quaternion.identity);
        }
        else
        {
            if (positionIndex == 2)
            {
                //delete walls
                Vector3Int rightGap1 = new Vector3Int((gridWidth / 2) - 1, 0, 0);
                Vector3Int rightGap2 = new Vector3Int((gridWidth / 2) - 1, -1, 0);
                startRoomWalls.SetTile(rightGap1, null);
                startRoomWalls.SetTile(rightGap2, null);

                Vector3Int enmyRightGap1 = new Vector3Int(-(enmyGridWidth / 2), 0, 0);
                Vector3Int enmyRightGap2 = new Vector3Int(-(enmyGridWidth / 2), -1, 0);
                wallTilemap.CompressBounds();
/*                Debug.Log(enmyRightGap1 + ", " + enmyRightGap2);
*/                wallTilemap.SetTile(enmyRightGap1, null);
                wallTilemap.SetTile(enmyRightGap2, null);
            }
            else
            {
                //delete walls
                Vector3Int leftGap1 = new Vector3Int(-(gridWidth / 2), 0, 0);
                Vector3Int leftGap2 = new Vector3Int(-(gridWidth / 2), -1, 0);
                startRoomWalls.SetTile(leftGap1, null);
                startRoomWalls.SetTile(leftGap2, null);

                Vector3Int enmyLeftGap1 = new Vector3Int((enmyGridWidth / 2) - 1, 0, 0);
                Vector3Int enmyLeftGap2 = new Vector3Int((enmyGridWidth / 2) - 1, -1, 0);
                Debug.Log(enmyLeftGap1 + ", " + enmyLeftGap2); //shld be 6
                wallTilemap.CompressBounds();
                wallTilemap.SetTile(enmyLeftGap1, null);
                wallTilemap.SetTile(enmyLeftGap2, null);
            }

            for (int i = 0; i < 16; i++)
            {
                spawnPoint += direction;
            }
            Instantiate(roomPrefabs[0], spawnPoint, Quaternion.identity);
        }
    }

    // Method to find a Tilemap with a specific tag in the children of a GameObject
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

public static class Direction3D
{
    public static List<Vector3Int> cardinalDirectionsList = new List<Vector3Int>
    {
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0)
    };

    public static Vector3Int GetRandomCardinalDirection(int index)
    {
        return cardinalDirectionsList[index];
    }
}
