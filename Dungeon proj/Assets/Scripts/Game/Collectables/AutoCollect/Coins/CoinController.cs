using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinController : MonoBehaviour
{
    public UnityEvent OnCoinAmtChanged;
    public int coinAmt;

    public void AddCoinAmt(int amount)
    {
        coinAmt += amount;
        OnCoinAmtChanged.Invoke();
    }

    public void DeductCoinAmt(int amount)
    {
        if(coinAmt >= amount)
        {
            coinAmt -= amount;
            OnCoinAmtChanged.Invoke();
        }
        else
        {
            Debug.Log("Not enough coins!");
        }
    }

    public void ChangeCoinAmt(int amount)
    {
        coinAmt = amount;
        OnCoinAmtChanged.Invoke();
    }
}
