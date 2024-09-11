using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public Player Player;
    public int Currency;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }

    public bool HaveEnoughMoney(int price)
    {
        if (price > Currency)
        {
            Debug.Log("Not enough money to buy");
            return false;
        }

        return true;
    }

    public void SpendCurrency(int price)
    {
        Currency = Currency - price;
    }

    public int CurrentCurrencyAmount()
    {
        return Currency;
    }
}