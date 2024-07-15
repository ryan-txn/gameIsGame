using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAwarenessController : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }

    public Vector2 DirectionToPlayer { get; private set; }

    [SerializeField]
    private float _playerAwarenessDistance;
    
    [SerializeField]
    private float _lossAwarenessDelay = 1f; // Delay before considering player out of sight

    private Transform _playerTransform;
    private GameObject _playerObject;
    private bool hasLineOfSight;
    private EnemyMovement _enemyMovement;
    private float _lastAwareTime; // Time when the enemy was last aware of the player

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerMovement>().transform;
        _playerObject = GameObject.FindGameObjectWithTag("Player");
        _enemyMovement = GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleLineOfSight();

        // Vector of how far player is and in what direction
        Vector2 enemyToPlayerVector = _playerTransform.position - transform.position;
        // Direction, normalized magnitude
        DirectionToPlayer = enemyToPlayerVector.normalized;

        if (enemyToPlayerVector.magnitude <= _playerAwarenessDistance)
        {
            if (gameObject.CompareTag("Player Collectable"))
            {
                AwareOfPlayer = true;
            }
            else if (hasLineOfSight)
            {
                Debug.DrawRay(transform.position, _playerObject.transform.position - transform.position, Color.green);
                AwareOfPlayer = true;
                _lastAwareTime = Time.time; // Update the last aware time
            }
            
        }
        else
        {
            Debug.DrawRay(transform.position, _playerObject.transform.position - transform.position, Color.red);

            // Check if enough time has passed since the last awareness
            if (Time.time - _lastAwareTime > _lossAwarenessDelay)
            {
                if (AwareOfPlayer && _enemyMovement != null)
                {
                    _enemyMovement.OnPlayerLeaveAwareness();
                }
                AwareOfPlayer = false;
            }
        }
    }

    private void HandleLineOfSight()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, _playerObject.transform.position - transform.position);

        if (ray.collider != null)
        {
            hasLineOfSight = ray.collider.CompareTag("Player");
        }
    }
}
