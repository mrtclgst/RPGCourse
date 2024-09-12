using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect")]
public class ItemEffect : ScriptableObject
{
    [TextArea]
    public string EffectDescription;


    public virtual void ExecuteEffect(Transform targetTransform)
    {
        Debug.Log("Effect Executed");
    }
}