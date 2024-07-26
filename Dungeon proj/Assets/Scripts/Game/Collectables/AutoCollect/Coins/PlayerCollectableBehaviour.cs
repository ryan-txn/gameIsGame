using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    [SerializeField]
    private bool _isCoin;

    public void OnCollected(GameObject player)
    {
        if (_isCoin)
        {
            FindObjectOfType<AudioManager>().PlaySFX("Coin sfx");
            player.GetComponent<CoinController>().AddCoinAmt(1);
        }
        else
        {
            FindObjectOfType<AudioManager>().PlaySFX("Stamina sfx");
            player.GetComponent<StaminaController>().RecoverStamina(10);
        }
    }
}
