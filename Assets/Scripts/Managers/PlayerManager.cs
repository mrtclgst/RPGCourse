using UnityEngine;

public class PlayerManager : MonoBehaviour, ISaveManager
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

    public void LoadData(GameData gameData)
    {
        Currency = gameData.Currency;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.Currency = Currency;
    }
}