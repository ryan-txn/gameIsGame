using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectableBehaviour : MonoBehaviour, interfaceCollectableBehaviour
{
    // Start is called before the first frsame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

      public void OnCollected(GameObject player)
    {
        player.GetComponent<CoinController>().AddCoinAmt(1);
    }
}
