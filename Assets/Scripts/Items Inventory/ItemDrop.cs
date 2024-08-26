using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int _possibleAmountOfItems;
    [SerializeField] private ItemData[] _possibleDrop;
    private List<ItemData> _dropList = new();
    [SerializeField] private GameObject _dropPrefab;

    public virtual void GenerateDrop()
    {
        for (int i = 0; i < _possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) <= _possibleDrop[i].DropChance)
            {
                _dropList.Add(_possibleDrop[i]);
            }
        }

        for (int i = 0; i < _possibleAmountOfItems; i++)
        {
            if (_dropList.Count > 0)
            {
                ItemData randomItem = _dropList[Random.Range(0, _dropList.Count - 1)];
                _dropList.Remove(randomItem);
                DropItem(randomItem);
            }
        }
    }

    protected virtual void DropItem(ItemData itemData)
    {
        GameObject newDrop = Instantiate(_dropPrefab, transform.position, Quaternion.identity);
        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(15, 18));

        newDrop.GetComponent<ItemObject>().SetupItem(itemData, randomVelocity);
    }
}