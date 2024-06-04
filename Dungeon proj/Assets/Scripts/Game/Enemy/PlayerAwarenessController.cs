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

    private Transform _playerTransform;
    private GameObject _playerObject;
    private bool hasLineOfSight;
    private EnemyMovement _enemyMovement;

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

        if ((enemyToPlayerVector.magnitude <= _playerAwarenessDistance) && hasLineOfSight)
        {
            Debug.DrawRay(transform.position, _playerObject.transform.position - transform.position, Color.green);
            AwareOfPlayer = true;
        }
        else
        {
            Debug.DrawRay(transform.position, _playerObject.transform.position - transform.position, Color.red);
            // Call the event when player leaves awareness
            if (AwareOfPlayer)
            {
                _enemyMovement.OnPlayerLeaveAwareness();
            }
            AwareOfPlayer = false;
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
