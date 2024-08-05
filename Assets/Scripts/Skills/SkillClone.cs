using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillClone : Skill
{
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private float _cloneDuration;
    [Space]
    [SerializeField] private bool _canAttack;

    internal void CreateClone(Transform clonePosition, Vector3 offset)
    {
        GameObject newClone = Instantiate(_clonePrefab);
        newClone.GetComponent<SkillCloneController>().SetupClone(clonePosition, _cloneDuration, _canAttack, offset);
    }
}