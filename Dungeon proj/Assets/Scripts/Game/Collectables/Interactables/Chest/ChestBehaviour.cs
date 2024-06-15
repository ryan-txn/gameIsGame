using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestBehaviour : MonoBehaviour
{
    private CollectableSpawner _collectableSpawner;
    private bool _chestOpened = false;
    private bool _collided;
    private Collider2D _collider; 

    [SerializeField]
    private Transform _spawnPoint;

    private void Awake()
    {
        _collectableSpawner = FindObjectOfType<CollectableSpawner>();
        _collider = GetComponent<Collider2D>(); 
    }

    private void Update()
    {
        if (_collided && _chestOpened)
        {
            _collectableSpawner.SpawnCollectable(_spawnPoint.position, CollectableSpawner.SpawnType.Weapon);
            _collider.enabled = false;
            Debug.Log("spawned weapon");
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.tag == "Player")
        {
            _collided = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {

        if (collider.gameObject.tag == "Player")
        {
            _collided = false;
        }
    }

    private void OnInteract(InputValue inputValue)
    {
        _chestOpened = inputValue.isPressed;
    }
}
