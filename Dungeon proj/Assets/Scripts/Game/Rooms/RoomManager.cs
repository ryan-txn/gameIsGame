using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] roomPrefabs;


    private void Awake()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        Vector3 worldPos = new Vector3(0, 15, 0);
        Instantiate(roomPrefabs[0], worldPos, Quaternion.identity);
    }
}
