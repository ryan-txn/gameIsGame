using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    [SerializeField]
    private GameObject portal;

    private List<string> bossScenes;

    private void Start()
    {
        bossScenes = new List<string> { "Boss 1", "Boss 2", "Boss 3" };
    }

    public GameObject Spawn()
    {
        GameObject Portal = Instantiate(portal, transform);
        return Portal;
    }


}
