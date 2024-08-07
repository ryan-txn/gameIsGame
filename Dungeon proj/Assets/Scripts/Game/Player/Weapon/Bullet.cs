using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    [SerializeField]
    private string onShootSfx;

    [SerializeField]
    private GameObject _destroyEffect; //destroy particle effect

    //EXPLOSIVE BULLET ATTRIBUTES
    [SerializeField]
    private bool _isExplosive = false;

    [SerializeField]
    private float _explosionRadius = 2.0f;

    [SerializeField]
    private float _explosionForce = 200.0f;

    [SerializeField]
    private LayerMask _enemyLayerMask; // Layer mask for enemies

    //BOUNCY BULLET ATTRIBUTES
    [SerializeField]
    private bool _isBouncy = false;

    [SerializeField]
    private int _maxNumberOfBounces;

    private int _bounceCount = 0;


    /*    
    * private void Update() 
    * {
    *    DestroyWhenOffScreen(); //ensures that this happens every frame
    * }
    */

    private Rigidbody2D _rigidbody2D;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (onShootSfx!= null)
        {
            FindObjectOfType<AudioManager>().PlaySFX(onShootSfx);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Enemy"))
        {
            HandleCollision(collision);
        }
    }

    private void HandleCollision(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            HealthController healthController = collision.collider.GetComponent<HealthController>();
            healthController.TakeDamage(_damage);
        }

        Rigidbody2D collisionRigidbody = collision.collider.GetComponent<Rigidbody2D>();

        if (collisionRigidbody != null)
        {
            if (_isExplosive)
            {
                Explode();
            }

            if (_isBouncy)
            {
                _bounceCount++;
                if (_bounceCount > _maxNumberOfBounces)
                {
                    HandleBulletDestroy(); // Destroy bullet after reaching max bounces
                }
            }
            else
            {
                HandleBulletDestroy(); // Destroy non-bouncy bullet after collision
            }
        }
    }

    private void HandleBulletDestroy()
    {
        if (_destroyEffect != null)
        {
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    private void Explode()
    {
        //Hit enemies in the set explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius, _enemyLayerMask);
        foreach (Collider2D collider in colliders)
        {
            Debug.Log("Collider detected: " + collider.gameObject.name);
            var enemy = collider.GetComponent<EnemyMovement>();

            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            HealthController healthController = collider.GetComponent<HealthController>();

            if (healthController != null && rb != null && enemy != null)
            {
                // Calculate the direction from the explosion to the object
                Vector2 explosionDirection = rb.position - (Vector2)transform.position;
                float distance = explosionDirection.magnitude;
                explosionDirection.Normalize();
                // interpolate the force magnitude based on the distance from the explosion center
                float forceMagnitude = Mathf.Lerp(_explosionForce, 0, distance / _explosionRadius);

                Debug.Log($"Applying force: {forceMagnitude} to {collider.gameObject.name} in direction {explosionDirection}");

                rb.AddForce(explosionDirection * forceMagnitude, ForceMode2D.Impulse);
                healthController.TakeDamage(_damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _explosionRadius);
    }


    /*    private void DestroyWhenOffScreen() 
        {
            Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

            if (screenPosition.x < 0 || screenPosition.x > _camera.pixelWidth ||
                    screenPosition.y < 0 || screenPosition.y > _camera.pixelHeight) 
            {
                Destroy(gameObject);
            }
        }*/
}