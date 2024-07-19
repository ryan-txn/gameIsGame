using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestBehaviour : MonoBehaviour
{
    private CollectableSpawner _collectableSpawner;
    private bool _chestOpened = false;
    private bool _interacted;
    private bool _collided;
    private Collider2D _collider; 
    private Animator _animator;

    [SerializeField]
    private Transform _spawnPoint;

    private void Awake()
    {
        _collectableSpawner = FindObjectOfType<CollectableSpawner>();
        _collider = GetComponent<Collider2D>(); 
        _animator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        SetAnimation();
    }
    
    private void Update()
    {
        if (_collided && _interacted)
        {
            _chestOpened = true;
            _collectableSpawner.SpawnCollectable(_spawnPoint.position, CollectableSpawner.SpawnType.Weapon);
            _collider.enabled = false;
            Debug.Log("spawned weapon");

            FindObjectOfType<AudioManager>().PlaySFX("Chest open");
            Debug.Log("Chest open sound played");
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
        _interacted = inputValue.isPressed;
    }

    private void SetAnimation()
    {
        _animator.SetBool("IsOpened", _chestOpened);
    }
}
