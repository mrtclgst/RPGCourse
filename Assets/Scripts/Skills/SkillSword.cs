using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSword : MonoBehaviour
{
    [Header("Skill Info")]
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Vector2 _launchDirection;
    [SerializeField] private float _swordGravityScale;
}