using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private float _damageAmount;

    //called every frame an enemy collides into something
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player weapon"))
        {
            return;
        }
        //if collide into player,
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ignore Laser") && collision.gameObject.CompareTag("Player"))
        {
            //call healthcontroller class and use take damage method
            var healthController = collision.gameObject.GetComponent<HealthController>();

            healthController.TakeDamage(_damageAmount);
            Debug.Log("Enemy hit player");
        }
    }
}
