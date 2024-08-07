using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    [SerializeField]
    private float _healthAmount;

    public void OnCollected(GameObject player)
    {
        //Play sound effect
        FindObjectOfType<AudioManager>().PlaySFX("Health sfx");
        //Increase player health
        player.GetComponent<HealthController>().AddHealth(_healthAmount);
    }
}
