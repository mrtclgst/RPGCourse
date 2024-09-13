using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> _keyList = new();
    [SerializeField] private List<TValue> _valueList = new();

    public void OnBeforeSerialize()
    {
        _keyList.Clear();
        _valueList.Clear();

        foreach (KeyValuePair<TKey, TValue> pair in this)
        {
            _keyList.Add(pair.Key);
            _valueList.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        this.Clear();
        if (_keyList.Count != _valueList.Count)
        {
            Debug.Log("Keys count isn't equal to values count");
        }

        for (int i = 0; i < _keyList.Count; i++)
        {
            this.Add(_keyList[i], _valueList[i]);
        }
    }
}