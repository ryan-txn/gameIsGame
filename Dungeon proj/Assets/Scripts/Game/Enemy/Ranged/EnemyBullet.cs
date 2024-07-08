using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private Camera _camera;

    [SerializeField]
    private int _damage;

    private void Awake() 
    {
        _camera = Camera.main;
    }

    /*    
    * private void Update() 
    * {
    *    DestroyWhenOffScreen(); //ensures that this happens every frame
    * }
    */
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
       if (collision.GetComponent<PlayerMovement>()) 
       {
            HealthController healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(_damage);
            Destroy(gameObject); //destroy bullet
       }

       if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.GetComponent<PlayerSwingWeapon>())
        {
            Destroy(gameObject); //destroy bullet
        }
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
