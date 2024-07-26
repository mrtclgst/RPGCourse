using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [SerializeField] private SkillDash _skillDash;
    [SerializeField] private SkillClone _skillClone;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }
    private void Start()
    {

    }
    internal SkillDash GetSkillDash()
    {
        return _skillDash;
    }
    internal SkillClone GetSkillClone()
    {
        return _skillClone;
    }
}
