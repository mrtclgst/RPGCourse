using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    private SpriteRenderer _spriteRenderer;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _itemData.Icon;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Inventory.Instance.AddItem(_itemData);
            Debug.Log("Picked up item " + _itemData.name);
            Destroy(gameObject);
        }
    }
}