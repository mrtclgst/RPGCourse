using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int _baseValue;
    [SerializeField] private List<int> _modifiers;

    internal void SetBaseValue(int value)
    {
        _baseValue = value;
    }
    internal int GetValue()
    {
        int value = _baseValue;
        for (int i = 0; i < _modifiers.Count; i++)
        {
            value += _modifiers[i];
        }
        return value;
    }
    internal void AddModifier(int modifier)
    {
        _modifiers.Add(modifier);
    }
    internal void RemoveModifier(int modifier)
    {
        _modifiers.Remove(modifier);
    }
}