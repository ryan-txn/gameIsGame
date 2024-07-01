using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyRoomPrefabs;

    [SerializeField]
    private GameObject vertCorridor;

    [SerializeField]
    private GameObject horCorridor;

    private Vector3Int rootRoomSpawn = new Vector3Int(0, 0, 0);
    private Vector3Int currentRoomSpawn = new Vector3Int(0, 0, 0);

    [SerializeField]
    private GameObject firstRoom;
    private GameObject secondRoom;

    private readonly string wallTilemapTag = "Wall";

    private void Awake()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        List<int> currentDirections = new List<int> { 0, 1, 2, 3 };

        int positionIndex = Random.Range(0, currentDirections.Count); //get position from 0-3
        currentDirections.RemoveAt(3 - positionIndex); //remove opposite index option
        secondRoom = enemyRoomPrefabs[0];
        GameObject rootRoom;
        bool isRoot = true;
        if (positionIndex == 0) rootRoom = SpawnRoomOnTop(isRoot, firstRoom, secondRoom, 15, wallTilemapTag, 9);
        else if (positionIndex == 1) rootRoom = SpawnRoomOnLeft(isRoot, firstRoom, secondRoom, 18, wallTilemapTag, 7);
        else if (positionIndex == 2) rootRoom = SpawnRoomOnRight(isRoot, firstRoom, secondRoom, 18, wallTilemapTag, 8);
        else rootRoom = SpawnRoomOnBottom(isRoot, firstRoom, secondRoom, 15, wallTilemapTag, 8);

        positionIndex = currentDirections[Random.Range(0, currentDirections.Count)];
        currentDirections.Remove(positionIndex);
        firstRoom = rootRoom;
        secondRoom = enemyRoomPrefabs[1];
        GameObject spawnedRoom;
        isRoot = false;
        if (positionIndex == 0) spawnedRoom = SpawnRoomOnTop(isRoot, firstRoom, secondRoom, 13, wallTilemapTag, 5); //13 ok, 5 ok
        else if (positionIndex == 1) spawnedRoom = SpawnRoomOnLeft(isRoot, firstRoom, secondRoom, 12, wallTilemapTag, 4); //12 ok, 4 ok
        else if (positionIndex == 2) spawnedRoom = SpawnRoomOnRight(isRoot, firstRoom, secondRoom, 12, wallTilemapTag, 5); //12 ok, 5 ok
        else spawnedRoom = SpawnRoomOnBottom(isRoot, firstRoom, secondRoom, 13, wallTilemapTag, 4); //13 ok, 4 ok

        positionIndex = currentDirections[Random.Range(0, currentDirections.Count)];
        currentDirections.Remove(positionIndex);
        firstRoom = rootRoom;
        secondRoom = enemyRoomPrefabs[2];
        isRoot = false;
        if (positionIndex == 0) spawnedRoom = SpawnRoomOnTop(isRoot, firstRoom, secondRoom, 15, wallTilemapTag, 7); //13 ok, 5 ok
        else if (positionIndex == 1) spawnedRoom = SpawnRoomOnLeft(isRoot, firstRoom, secondRoom, 14, wallTilemapTag, 6); //12 ok, 4 ok
        else if (positionIndex == 2) spawnedRoom = SpawnRoomOnRight(isRoot, firstRoom, secondRoom, 14, wallTilemapTag, 7); //12 ok, 5 ok
        else spawnedRoom = SpawnRoomOnBottom(isRoot, firstRoom, secondRoom, 15, wallTilemapTag, 6); //13 ok, 4 ok

        //sometimes spawns same place as chest room
        positionIndex = currentDirections[Random.Range(0, currentDirections.Count)];
        firstRoom = rootRoom;
        secondRoom = enemyRoomPrefabs[0];
        isRoot = true;
        if (positionIndex == 0) SpawnRoomOnTop(isRoot, firstRoom, secondRoom, 17, wallTilemapTag, 9);
        else if (positionIndex == 1) SpawnRoomOnLeft(isRoot, firstRoom, secondRoom, 15, wallTilemapTag, 7);
        else if (positionIndex == 2) SpawnRoomOnRight(isRoot, firstRoom, secondRoom, 15, wallTilemapTag, 8);
        else SpawnRoomOnBottom(isRoot, firstRoom, secondRoom, 17, wallTilemapTag, 8);
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

    GameObject SpawnRoomOnTop(bool isRoot, GameObject firstRoom, GameObject secondRoom, int distanceFromFirstRoom,
                        string wallTilemapTag, int corrDistance)
    {
        //spawn in second room
        Vector3Int direction = new Vector3Int(0, 1, 0);
        if (isRoot)
        {
            rootRoomSpawn += direction * distanceFromFirstRoom;
            currentRoomSpawn = rootRoomSpawn;
        }
        else
        {
            currentRoomSpawn = rootRoomSpawn + direction * distanceFromFirstRoom;
        }
        GameObject RoomInstance = Instantiate(secondRoom, currentRoomSpawn, Quaternion.identity);

        //delete first room walls
        Tilemap firstRoomWalls = FindTilemapWithTag(firstRoom, wallTilemapTag);
        firstRoomWalls.CompressBounds();
        int firstRoomHeight = firstRoomWalls.size.y;
        Vector3Int topGap1 = new(-1, (firstRoomHeight / 2) - 1, 0);
        Vector3Int topGap2 = new(0, (firstRoomHeight / 2) - 1, 0);
        firstRoomWalls.SetTile(topGap1, null);
        firstRoomWalls.SetTile(topGap2, null);
        Debug.Log(topGap1 + ", " + topGap2);

        //find second room wall tilemap
        Tilemap wallTilemap = FindTilemapWithTag(RoomInstance, wallTilemapTag);

        //checking second room wall tilemap dimensions
        wallTilemap.CompressBounds();
        int secondGridWidth = wallTilemap.size.x;
        int secondGridHeight = wallTilemap.size.y;
        Debug.Log(secondGridHeight + ", " + secondGridWidth);

        //checking second room wall tilemap tiles to be deleted
        Vector3Int secondBottomGap1 = new(-1, -(secondGridHeight / 2), 0);
        Vector3Int secondBottomGap2 = new(0, -(secondGridHeight / 2), 0);
        Debug.Log(secondBottomGap1 + ", " + secondBottomGap2); // shld be -8

        //delete second room walls
        wallTilemap.CompressBounds();
        wallTilemap.SetTile(secondBottomGap1, null);
        wallTilemap.SetTile(secondBottomGap2, null);

        //spawn corridor
        Vector3 corrSpawnPoint = currentRoomSpawn - (direction * corrDistance);
        Instantiate(vertCorridor, corrSpawnPoint, Quaternion.identity);

        return RoomInstance;
    }


    GameObject SpawnRoomOnLeft(bool isRoot, GameObject firstRoom, GameObject secondRoom, int distanceFromFirstRoom,
                    string wallTilemapTag, int corrDistance)
    {
        //spawn in second room
        Vector3Int direction = new Vector3Int(-1, 0, 0);
        if (isRoot)
        {
            rootRoomSpawn += direction * distanceFromFirstRoom;
            currentRoomSpawn = rootRoomSpawn;
        }
        else
        {
            currentRoomSpawn = rootRoomSpawn + direction * distanceFromFirstRoom;
        }
        GameObject RoomInstance = Instantiate(secondRoom, currentRoomSpawn, Quaternion.identity);

        //delete first room walls
        Tilemap firstRoomWalls = FindTilemapWithTag(firstRoom, wallTilemapTag);
        firstRoomWalls.CompressBounds();
        int firstRoomWidth = firstRoomWalls.size.x;
        Vector3Int leftGap1 = new Vector3Int(-(firstRoomWidth / 2), 0, 0);
        Vector3Int leftGap2 = new Vector3Int(-(firstRoomWidth / 2), -1, 0);
        firstRoomWalls.SetTile(leftGap1, null);
        firstRoomWalls.SetTile(leftGap2, null);
        Debug.Log(leftGap1 + ", " + leftGap2);

        //find second room wall tilemap
        Tilemap wallTilemap = FindTilemapWithTag(RoomInstance, wallTilemapTag);

        //checking second room wall tilemap dimensions
        wallTilemap.CompressBounds();
        int secondGridWidth = wallTilemap.size.x;
        int secondGridHeight = wallTilemap.size.y;
        Debug.Log(secondGridHeight + ", " + secondGridWidth);

        //checking second room wall tilemap tiles to be deleted
        Vector3Int enmyLeftGap1 = new Vector3Int((secondGridWidth / 2) - 1, 0, 0);
        Vector3Int enmyLeftGap2 = new Vector3Int((secondGridWidth / 2) - 1, -1, 0);
        Debug.Log(enmyLeftGap1 + ", " + enmyLeftGap2); //shld be 6

        //delete second room walls
        wallTilemap.CompressBounds();
        wallTilemap.SetTile(enmyLeftGap1, null);
        wallTilemap.SetTile(enmyLeftGap2, null);

        //spawn corridor
        Vector3 corrSpawnPoint = currentRoomSpawn - (direction * corrDistance);
        Instantiate(horCorridor, corrSpawnPoint, Quaternion.identity);

        return RoomInstance;
    }

    GameObject SpawnRoomOnRight(bool isRoot, GameObject firstRoom, GameObject secondRoom, int distanceFromFirstRoom,
                string wallTilemapTag, int corrDistance)
    {
        //spawn in second room
        Vector3Int direction = new Vector3Int(1, 0, 0);
        if (isRoot)
        {
            rootRoomSpawn += direction * distanceFromFirstRoom;
            currentRoomSpawn = rootRoomSpawn;
        }
        else
        {
            currentRoomSpawn = rootRoomSpawn + direction * distanceFromFirstRoom;
        }
        GameObject RoomInstance = Instantiate(secondRoom, currentRoomSpawn, Quaternion.identity);

        //delete first room walls
        Tilemap firstRoomWalls = FindTilemapWithTag(firstRoom, wallTilemapTag);
        firstRoomWalls.CompressBounds();
        int firstRoomWidth = firstRoomWalls.size.x;
        Vector3Int rightGap1 = new Vector3Int((firstRoomWidth / 2) - 1, 0, 0);
        Vector3Int rightGap2 = new Vector3Int((firstRoomWidth / 2) - 1, -1, 0);
        firstRoomWalls.SetTile(rightGap1, null);
        firstRoomWalls.SetTile(rightGap2, null);
        Debug.Log(rightGap1 + ", " + rightGap2);

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

        //spawn corridor
        Vector3 corrSpawnPoint = currentRoomSpawn - (direction * corrDistance);
        Instantiate(horCorridor, corrSpawnPoint, Quaternion.identity);
        return RoomInstance;
    }

    GameObject SpawnRoomOnBottom(bool isRoot, GameObject firstRoom, GameObject secondRoom, int distanceFromFirstRoom,
            string wallTilemapTag, int corrDistance)
    {
        //spawn in second room
        Vector3Int direction = new Vector3Int(0, -1, 0);
        if (isRoot)
        {
            rootRoomSpawn += direction * distanceFromFirstRoom;
            currentRoomSpawn = rootRoomSpawn;
        }
        else
        {
            currentRoomSpawn = rootRoomSpawn + direction * distanceFromFirstRoom;
        }
        GameObject RoomInstance = Instantiate(secondRoom, currentRoomSpawn, Quaternion.identity);

        //delete first room walls
        Tilemap firstRoomWalls = FindTilemapWithTag(firstRoom, wallTilemapTag);
        firstRoomWalls.CompressBounds();
        int firstRoomHeight = firstRoomWalls.size.y;
        Vector3Int bottomGap1 = new(-1, -(firstRoomHeight / 2), 0);
        Vector3Int bottomGap2 = new(0, -(firstRoomHeight / 2), 0);
        firstRoomWalls.SetTile(bottomGap1, null);
        firstRoomWalls.SetTile(bottomGap2, null);
        Debug.Log(bottomGap1 + ", " + bottomGap2);

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

        //spawn corridor
        Vector3 corrSpawnPoint = currentRoomSpawn - (direction * corrDistance);
        Instantiate(vertCorridor, corrSpawnPoint, Quaternion.identity);
        return RoomInstance;
    }
}
