using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinController : MonoBehaviour
{
    public UnityEvent OnCoinAmtChanged;
    public int coinAmt {get; private set;}

    public void AddCoinAmt(int amount)
    {
        coinAmt += amount;
        OnCoinAmtChanged.Invoke();
    }

}
