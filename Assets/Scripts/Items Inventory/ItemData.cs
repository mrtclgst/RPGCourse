using System.Text;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType ItemType;
    public string ItemName;
    public Sprite Icon;
    public string ItemID;

    [Range(0, 100)]
    public float DropChance;

    protected StringBuilder _sb = new StringBuilder();
    public virtual string GetDescription()
    {
        return "";
    }
    private void OnValidate()
    {
#if UNITY_EDITOR
        string path = AssetDatabase.GetAssetPath(this);
        ItemID = AssetDatabase.AssetPathToGUID(path);
#endif
    }
}

public enum ItemType
{
    Material,
    Equipment,
}