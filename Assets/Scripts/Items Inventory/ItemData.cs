using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType;
    public string ItemName;
    public Sprite Icon;
}

public enum ItemType
{
    Material,
    Equipment,
}