using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private ItemData _itemData;
    private SpriteRenderer _spriteRenderer;

    internal void PickUpItem()
    {
        Inventory.Instance.AddItem(_itemData);
        Destroy(gameObject);
    }
    internal void SetupItem(ItemData itemData, Vector2 velocity)
    {
        _itemData = itemData;
        _rb.velocity = velocity;
        SetVisual();
    }

    private void SetVisual()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _itemData.Icon;
    }
}