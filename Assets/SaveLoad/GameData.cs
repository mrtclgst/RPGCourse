using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int Currency;
    public SerializableDictionary<string, int> Inventory;
    public SerializableDictionary<string, bool> SkillTree;
    public List<string> EquipmentIDList;

    public SerializableDictionary<string, bool> Checkpoints;
    public string ClosestCheckpointID;

    public GameData()
    {
        this.Currency = 0;
        Inventory = new SerializableDictionary<string, int>();
        SkillTree = new SerializableDictionary<string, bool>();
        EquipmentIDList = new List<string>();

        Checkpoints = new SerializableDictionary<string, bool>();
        ClosestCheckpointID = string.Empty;
    }
}
