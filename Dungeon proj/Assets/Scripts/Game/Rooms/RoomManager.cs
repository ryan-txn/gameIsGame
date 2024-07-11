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

    private readonly string WALL_STRING = "Wall";

    private readonly int fixedRoomDistance = 30;
    private readonly int corrWidth = 4;


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
        int room_distance;
        int corr_distance;
        Vector3Int direction = ReturnDirection(positionIndex);
        room_distance = FindDistanceRooms(firstRoom, secondRoom, direction);
        corr_distance = FindDistanceCorr(secondRoom, direction);
        rootRoom = SpawnRoom(direction, isRoot, firstRoom, secondRoom, fixedRoomDistance, WALL_STRING, corr_distance);

        positionIndex = currentDirections[Random.Range(0, currentDirections.Count)];
        currentDirections.Remove(positionIndex);
        firstRoom = rootRoom;
        secondRoom = enemyRoomPrefabs[1];
        GameObject spawnedRoom;
        isRoot = false;
        direction = ReturnDirection(positionIndex);
        room_distance = FindDistanceRooms(firstRoom, secondRoom, direction);
        corr_distance = FindDistanceCorr(secondRoom, direction);
        spawnedRoom = SpawnRoom(direction, isRoot, firstRoom, secondRoom, fixedRoomDistance, WALL_STRING, corr_distance);

        positionIndex = currentDirections[Random.Range(0, currentDirections.Count)];
        currentDirections.Remove(positionIndex);
        firstRoom = rootRoom;
        secondRoom = enemyRoomPrefabs[2];
        isRoot = false;
        direction = ReturnDirection(positionIndex);
        room_distance = FindDistanceRooms(firstRoom, secondRoom, direction);
        corr_distance = FindDistanceCorr(secondRoom, direction);
        spawnedRoom = SpawnRoom(direction, isRoot, firstRoom, secondRoom, fixedRoomDistance, WALL_STRING, corr_distance);

        positionIndex = currentDirections[Random.Range(0, currentDirections.Count)];
        firstRoom = rootRoom;
        secondRoom = enemyRoomPrefabs[0];
        isRoot = true;
        direction = ReturnDirection(positionIndex);
        room_distance = FindDistanceRooms(firstRoom, secondRoom, direction);
        corr_distance = FindDistanceCorr(secondRoom, direction);
        rootRoom = SpawnRoom(direction, isRoot, firstRoom, secondRoom, fixedRoomDistance, WALL_STRING, corr_distance);

        currentDirections = new List<int> { 0, 1, 2, 3 };
        currentDirections.RemoveAt(3 - positionIndex); //remove opposite index option
        positionIndex = currentDirections[Random.Range(0, currentDirections.Count)]; //get position from 0-3
        firstRoom = rootRoom;
        secondRoom = enemyRoomPrefabs[0];
        isRoot = true;
        direction = ReturnDirection(positionIndex);
        room_distance = FindDistanceRooms(firstRoom, secondRoom, direction);
        corr_distance = FindDistanceCorr(secondRoom, direction);
        rootRoom = SpawnRoom(direction, isRoot, firstRoom, secondRoom, fixedRoomDistance, WALL_STRING, corr_distance);
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

    Vector3Int ReturnDirection(int positionIndex)
    {
        if (positionIndex == 0) return new Vector3Int(0, 1, 0);
        else if (positionIndex == 1) return new Vector3Int(-1, 0, 0);
        else if (positionIndex == 2) return new Vector3Int(1, 0, 0);
        else return new Vector3Int(0, -1, 0);
    }

    int FindDistanceRooms(GameObject firstRoom, GameObject secondRoom, Vector3 direction)
    {
        Tilemap firstTilemap = FindTilemapWithTag(firstRoom, WALL_STRING);
        Tilemap secondTilemap = FindTilemapWithTag(secondRoom, WALL_STRING);
        int distance;
        if (direction == new Vector3Int(0, 1, 0) || direction == new Vector3Int(0, -1, 0))
        {
            firstTilemap.CompressBounds();
            int firstRoomHeight = firstTilemap.size.y;
            secondTilemap.CompressBounds();
            int secondRoomHeight = secondTilemap.size.y;
            distance = (firstRoomHeight / 2) + (secondRoomHeight / 2);

        }
        else
        {
            firstTilemap.CompressBounds();
            int firstRoomWidth = firstTilemap.size.x;
            secondTilemap.CompressBounds();
            int secondRoomWidth = secondTilemap.size.x;
            distance = (firstRoomWidth / 2) + (secondRoomWidth / 2);
        }

        return distance;
    }

    int FindDistanceCorr(GameObject Room, Vector3 direction)
    {
        Tilemap Tilemap = FindTilemapWithTag(Room, WALL_STRING);
        int distance;
        Tilemap.CompressBounds();
        int RoomHeight = Tilemap.size.y;
        int RoomWidth = Tilemap.size.x;
        if (direction == new Vector3Int(0, 1, 0)) distance = (RoomHeight / 2) + 1;
        else if (direction == new Vector3Int(0, -1, 0)) distance = (RoomHeight / 2);
        else if (direction == new Vector3Int(1, 0, 0)) distance = (RoomWidth / 2) + 1;
        else distance = (RoomWidth / 2);

        return distance;
    }

    GameObject SpawnRoom(Vector3Int direction, bool isRoot, GameObject firstRoom, GameObject secondRoom, int distanceFromFirstRoom,
                string wallTilemapTag, int corrDistance)
    {
        GameObject corridor;

        //spawn in second room
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
        Tilemap firstRoomWalls = FindTilemapWithTag(firstRoom, WALL_STRING);
        firstRoomWalls.CompressBounds();
        int firstRoomDimension;

        Tilemap secondRoomWalls = FindTilemapWithTag(RoomInstance, WALL_STRING);
        secondRoomWalls.CompressBounds();
        int secondRoomDimension;

        if (direction == new Vector3Int(0, 1, 0) || direction == new Vector3Int(0, -1, 0))
        {
            firstRoomDimension = firstRoomWalls.size.y;
            secondRoomDimension = secondRoomWalls.size.y;
            corridor = vertCorridor;
        }
        else
        {
            firstRoomDimension = firstRoomWalls.size.x;
            secondRoomDimension = secondRoomWalls.size.x;

            corridor = horCorridor;
        }
        DeleteRoomWalls(firstRoomWalls, firstRoomDimension, direction);

        //delete second room walls
        Vector3Int oppDirection = direction * -1;
        DeleteRoomWalls(secondRoomWalls, secondRoomDimension, oppDirection);

        //spawn corridor
        Vector3 corrSpawnPoint = currentRoomSpawn - (direction * corrDistance);
        int corrLength = fixedRoomDistance - FindDistanceRooms(firstRoom, secondRoom, direction);
        for (int i = 0; i < corrLength; i++)
        {
            Instantiate(corridor, corrSpawnPoint, Quaternion.identity);
            corrSpawnPoint -= direction;
        }

        return RoomInstance;
    }

    void DeleteRoomWalls(Tilemap roomWalls, int roomDimension, Vector3Int direction)
    {
        Vector3Int Gap = new Vector3Int(0, 0, 0);



        if (direction == new Vector3Int(0, 1, 0))
        {
            Gap += direction * ((roomDimension / 2) - 1); // initial spawnpoint
            Vector3Int tempGap = Gap;
            for (int i = 0; i < corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap += new Vector3Int(1, 0, 0);
            }
            tempGap = Gap;
            for (int i = 0; i <= corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap -= new Vector3Int(1, 0, 0);
            }
        }
        else if (direction == new Vector3Int(0, -1, 0))
        {
            Gap += direction * ((roomDimension / 2)); // initial spawnpoint
            Vector3Int tempGap = Gap;
            for (int i = 0; i < corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap += new Vector3Int(1, 0, 0);
            }
            tempGap = Gap;
            for (int i = 0; i <= corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap -= new Vector3Int(1, 0, 0);
            }
        }
        else if (direction == new Vector3Int(1, 0, 0))
        {
            Gap += direction * ((roomDimension / 2) - 1); // initial spawnpoint
            Vector3Int tempGap = Gap;
            for (int i = 0; i < corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap += new Vector3Int(0, 1, 0);
            }
            tempGap = Gap;
            for (int i = 0; i <= corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap -= new Vector3Int(0, 1, 0);
            }
        }
        else
        {
            Gap += direction * ((roomDimension / 2)); // initial spawnpoint
            Vector3Int tempGap = Gap;
            for (int i = 0; i < corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap += new Vector3Int(0, 1, 0);
            }
            tempGap = Gap;
            for (int i = 0; i <= corrWidth / 2; i++)
            {
                roomWalls.SetTile(tempGap, null);
                tempGap -= new Vector3Int(0, 1, 0);
            }
        }
    }
}