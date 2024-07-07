using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    [SerializeField]
    private bool _isExplosive = false;

    [SerializeField]
    private float _explosionRadius = 2.0f;

    [SerializeField]
    private float _explosionForce = 200.0f;

    [SerializeField]
    private GameObject _explosionEffect; //visual effect

    [SerializeField]
    private LayerMask _enemyLayerMask; // Layer mask for enemies

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
    private float _initialSpeed;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _initialSpeed = _rigidbody2D.velocity.magnitude;
        _rigidbody2D.velocity = _rigidbody2D.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.GetComponent<EnemyMovement>())
        {
            HandleCollision(collision);
        }
    }

    private void HandleCollision(Collision2D collision)
    {
        if (collision.collider.GetComponent<EnemyMovement>())
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
                    Destroy(gameObject); // Destroy bullet after reaching max bounces
                }
                else
                {
                    Bounce(collision);
                }
            }
            else
            {
                Destroy(gameObject); // Destroy non-bouncy bullet after collision
            }
        }
    }

    private void Explode()
    {
        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        }

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

    private void Bounce(Collision2D collision)
    {
        if (_rigidbody2D != null)
        {
            // Get reflection vector using normal of contact point
            Vector2 reflectDirection = Vector2.Reflect(_rigidbody2D.velocity.normalized, collision.contacts[0].normal);

            // Blend the incoming direction with the reflected direction
            float blendFactor = 0.7f; // Adjust this to smooth out the bounce angle
            Vector2 smoothedDirection = Vector2.Lerp(_rigidbody2D.velocity.normalized, reflectDirection, blendFactor).normalized;

            _rigidbody2D.velocity = smoothedDirection * (_initialSpeed * 0.75f);

            StartCoroutine(DestroyAfterTime());
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject);
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