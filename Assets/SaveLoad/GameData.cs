using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int Currency;
    public SerializableDictionary<string, int> Inventory;

    public GameData()
    {
        this.Currency = 0;
        Inventory = new SerializableDictionary<string, int>();
    }
}
