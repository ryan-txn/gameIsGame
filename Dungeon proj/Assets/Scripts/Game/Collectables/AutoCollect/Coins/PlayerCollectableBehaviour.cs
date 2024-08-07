using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    [SerializeField]
    private bool _isCoin;

    [SerializeField]
    private int _coinValue = 1;

    [SerializeField]
    private int _staminaValue = 15;

    public void OnCollected(GameObject player)
    {
        if (_isCoin)
        {
            FindObjectOfType<AudioManager>().PlaySFX("Coin sfx");
            player.GetComponent<CoinController>().AddCoinAmt(_coinValue);
        }
        else
        {
            FindObjectOfType<AudioManager>().PlaySFX("Stamina sfx");
            player.GetComponent<StaminaController>().RecoverStamina(_staminaValue);
        }
    }
}
