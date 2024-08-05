using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : Skill
{
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private float _crystalDuration;
    private GameObject _currentCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private bool _canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool _canMoveToEnemy;
    [SerializeField] private float _moveSpeed;



    internal override void UseSkill()
    {
        base.UseSkill();

        if (!_currentCrystal)
        {
            _currentCrystal = Instantiate(_crystalPrefab, _player.transform.position, Quaternion.identity);
            SkillCrystalController crystalController = _currentCrystal.GetComponent<SkillCrystalController>();
            crystalController.SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed);
        }
        else
        {
            Vector2 playerPos = _player.transform.position;
            _player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPos;
            _currentCrystal.GetComponent<SkillCrystalController>()?.CrystalExplosionLogic();
        }
    }
}