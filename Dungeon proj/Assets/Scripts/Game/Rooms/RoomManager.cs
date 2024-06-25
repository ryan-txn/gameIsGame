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
        string wallTilemapTag = "Wall";

        int positionIndex = 3; //Random.Range(0, 4);
        var direction = Direction3D.GetRandomCardinalDirection(positionIndex);

        if (positionIndex == 0) //ROOM ON TOP
        {
            spawnPoint += (direction * 13);
            GameObject RoomInstance = Instantiate(roomPrefabs[0], spawnPoint, Quaternion.identity);

            //delete room 0 walls
            Vector3Int topGap1 = new(-1, (gridHeight / 2) - 1, 0);
            Vector3Int topGap2 = new(0, (gridHeight / 2) - 1, 0);
            startRoomWalls.SetTile(topGap1, null);
            startRoomWalls.SetTile(topGap2, null);

            //find room 1 wall tilemap
            Tilemap wallTilemap = FindTilemapWithTag(RoomInstance, wallTilemapTag);

            //checking room 1 wall tilemap dimensions
            wallTilemap.CompressBounds();
            int enmyGridWidth = wallTilemap.size.x;
            int enmyGridHeight = wallTilemap.size.y;
            Debug.Log(enmyGridHeight + ", " + enmyGridWidth);

            //checking room 1 wall tilemap tiles to be deleted
            Vector3Int enmyBottomGap1 = new(-1, -(enmyGridHeight / 2), 0);
            Vector3Int enmyBottomGap2 = new(0, -(enmyGridHeight / 2), 0);
            Debug.Log(enmyBottomGap1 + ", " + enmyBottomGap2); // shld be -8

            //delete appropriate room 1 walls
            wallTilemap.CompressBounds();
            wallTilemap.SetTile(enmyBottomGap1, null);
            wallTilemap.SetTile(enmyBottomGap2, null);
        }
        else if (positionIndex == 1)
        { //ROOM ON BOTTOM
            spawnPoint += (direction * 13);
            GameObject RoomInstance = Instantiate(roomPrefabs[0], spawnPoint, Quaternion.identity);

            //delete room 0 walls
            Vector3Int bottomGap1 = new(-1, -(gridHeight / 2), 0);
            Vector3Int bottomGap2 = new(0, -(gridHeight / 2), 0);
            startRoomWalls.SetTile(bottomGap1, null);
            startRoomWalls.SetTile(bottomGap2, null);

            //find room 1 wall tilemap
            Tilemap wallTilemap = FindTilemapWithTag(RoomInstance, wallTilemapTag);

            //checking room 1 wall tilemap dimensions
            wallTilemap.CompressBounds();
            int enmyGridWidth = wallTilemap.size.x;
            int enmyGridHeight = wallTilemap.size.y;
            Debug.Log(enmyGridHeight + ", " + enmyGridWidth);

            //checking room 1 wall tilemap tiles to be deleted
            Vector3Int enmyTopGap1 = new(-1, (enmyGridHeight / 2) - 1, 0);
            Vector3Int enmyTopGap2 = new(0, (enmyGridHeight / 2) - 1, 0);
            Debug.Log(enmyTopGap1 + ", " + enmyTopGap2); // shld be 7

            //delete appropriate room 1 walls
            wallTilemap.CompressBounds();
            wallTilemap.SetTile(enmyTopGap1, null);
            wallTilemap.SetTile(enmyTopGap2, null);
        }
        else if (positionIndex == 2) //ROOM ON RIGHT
        {
            spawnPoint += (direction * 16);
            GameObject RoomInstance = Instantiate(roomPrefabs[0], spawnPoint, Quaternion.identity);

            //delete room 0 walls
            Vector3Int rightGap1 = new Vector3Int((gridWidth / 2) - 1, 0, 0);
            Vector3Int rightGap2 = new Vector3Int((gridWidth / 2) - 1, -1, 0);
            startRoomWalls.SetTile(rightGap1, null);
            startRoomWalls.SetTile(rightGap2, null);

            //find room 1 wall tilemap
            Tilemap wallTilemap = FindTilemapWithTag(RoomInstance, wallTilemapTag);

            //checking room 1 wall tilemap dimensions
            wallTilemap.CompressBounds();
            int enmyGridWidth = wallTilemap.size.x;
            int enmyGridHeight = wallTilemap.size.y;
            Debug.Log(enmyGridHeight + ", " + enmyGridWidth);

            //checking room 1 wall tilemap tiles to be deleted
            Vector3Int enmyRightGap1 = new Vector3Int(-(enmyGridWidth / 2), 0, 0);
            Vector3Int enmyRightGap2 = new Vector3Int(-(enmyGridWidth / 2), -1, 0);
            Debug.Log(enmyRightGap1 + ", " + enmyRightGap2); // shld be

            //delete appropriate room 1 walls
            wallTilemap.CompressBounds();
            wallTilemap.SetTile(enmyRightGap1, null);
            wallTilemap.SetTile(enmyRightGap2, null);
        }
        else //ROOM ON LEFT
        {
            spawnPoint += (direction * 16);
            GameObject RoomInstance = Instantiate(roomPrefabs[0], spawnPoint, Quaternion.identity);

            //delete room 0 walls
            Vector3Int leftGap1 = new Vector3Int(-(gridWidth / 2), 0, 0);
            Vector3Int leftGap2 = new Vector3Int(-(gridWidth / 2), -1, 0);
            startRoomWalls.SetTile(leftGap1, null);
            startRoomWalls.SetTile(leftGap2, null);

            //find room 1 wall tilemap
            Tilemap wallTilemap = FindTilemapWithTag(RoomInstance, wallTilemapTag);

            //checking room 1 wall tilemap dimensions
            wallTilemap.CompressBounds();
            int enmyGridWidth = wallTilemap.size.x;
            int enmyGridHeight = wallTilemap.size.y;
            Debug.Log(enmyGridHeight + ", " + enmyGridWidth);

            //checking room 1 wall tilemap tiles to be deleted
            Vector3Int enmyLeftGap1 = new Vector3Int((enmyGridWidth / 2) - 1, 0, 0);
            Vector3Int enmyLeftGap2 = new Vector3Int((enmyGridWidth / 2) - 1, -1, 0);
            Debug.Log(enmyLeftGap1 + ", " + enmyLeftGap2); //shld be 6

            //delete appropriate room 1 walls
            wallTilemap.CompressBounds();
            wallTilemap.SetTile(enmyLeftGap1, null);
            wallTilemap.SetTile(enmyLeftGap2, null);
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
