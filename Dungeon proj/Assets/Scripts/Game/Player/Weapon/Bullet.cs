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
    private float _explosionRadius = 5.0f;

    [SerializeField]
    private GameObject _explosionEffect; //visual effect

    /*    
    * private void Update() 
    * {
    *    DestroyWhenOffScreen(); //ensures that this happens every frame
    * }
    */
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.GetComponent<EnemyMovement>()) 
       {
            HealthController healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(_damage);
            if (_isExplosive)
            {
                Explode();
            }
            Destroy(gameObject); //destroy bullet
       }

       if (collision.CompareTag("Wall"))
        {
            if (_isExplosive)
            {
                Explode();
            }
            Destroy(gameObject);
        }
    }

    private void Explode()
    {
        if (_explosionEffect != null)
        {
            Instantiate(_explosionEffect, transform.position, Quaternion.identity);
        }
        //Hit enemies in the set explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            HealthController healthController = collider.GetComponent<HealthController>();
            if (healthController != null && collider.GetComponent<EnemyMovement>() != null)
            {
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
