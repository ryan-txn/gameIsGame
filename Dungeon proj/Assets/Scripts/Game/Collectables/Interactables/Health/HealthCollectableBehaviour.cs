using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    [SerializeField]
    private float _healthAmount;

    public void OnCollected(GameObject player)
    {
        FindObjectOfType<AudioManager>().PlaySFX("Health sfx");
        player.GetComponent<HealthController>().AddHealth(_healthAmount);
    }
}
