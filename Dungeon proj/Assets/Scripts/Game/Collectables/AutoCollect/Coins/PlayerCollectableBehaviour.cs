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
            player.GetComponent<CoinController>().AddCoinAmt(1);
        }
        else
        {
            player.GetComponent<StaminaController>().RecoverStamina(10);
        }
    }
}
