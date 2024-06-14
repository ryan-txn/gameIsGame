using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    public void OnCollected(GameObject player)
    {
        player.GetComponent<CoinController>().AddCoinAmt(1);
    }
}
