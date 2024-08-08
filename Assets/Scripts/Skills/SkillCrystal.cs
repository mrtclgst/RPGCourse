using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCrystal : Skill
{
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private float _crystalDuration;
    [SerializeField] private int _damage;

    [Header("Crystal Mirage")]
    [SerializeField] private bool _canSpawnClone;

    private GameObject _currentCrystal;

    [Header("Explosive Crystal")]
    [SerializeField] private bool _canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool _canMoveToEnemy;
    [SerializeField] private float _moveSpeed;

    [Header("Multi Stacking Crystals")]
    [SerializeField] private bool _canUseMultiStackCrystal;
    [SerializeField] private int _amountOfStack;
    [SerializeField] private float _cooldownOfMultiCrystal;
    [SerializeField] private float _useTimeWindow;
    private float _multiCrystalTimer;
    [SerializeField] private List<GameObject> _crystalList = new();
    protected override void Update()
    {
        base.Update();
        if (_canUseMultiStackCrystal)
        {
            _multiCrystalTimer -= Time.deltaTime;
        }
    }

    internal override void UseSkill()
    {
        base.UseSkill();

        if (_canUseMultiStackCrystal)
        {
            if (CanUseMultiCrystal())
                return;

            return;
        }
        if (!_currentCrystal)
        {
            CreateCrystal();
        }
        else
        {
            if (_canMoveToEnemy)
                return;

            Vector2 playerPos = _player.transform.position;
            _player.transform.position = _currentCrystal.transform.position;
            _currentCrystal.transform.position = playerPos;

            if (_canSpawnClone)
            {
                SkillManager.Instance.GetSkillClone().CreateClone(_currentCrystal.transform, Vector3.zero);
                Destroy(_currentCrystal);
            }
            else
            {
                _currentCrystal.GetComponent<SkillCrystalController>()?.CrystalExplosionLogic();
            }
        }
    }

    internal void CreateCrystal()
    {
        _currentCrystal = Instantiate(_crystalPrefab, _player.transform.position, Quaternion.identity);
        SkillCrystalController crystalController = _currentCrystal.GetComponent<SkillCrystalController>();
        crystalController.SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, FindClosestEnemy(_currentCrystal.transform), _damage);
    }

    internal void CurrentCrystalChooseRandomEnemy()
    {
        _currentCrystal.GetComponent<SkillCrystalController>().ChooseRandomEnemy();
    }

    private bool CanUseMultiCrystal()
    {
        if (_canUseMultiStackCrystal)
        {
            if (_crystalList.Count > 0 && _multiCrystalTimer < 0f)
            {
                if (_amountOfStack == _crystalList.Count)
                    Invoke("ResetAbility", _useTimeWindow);

                _multiCrystalTimer = 0f;
                GameObject crystalToSpawn = _crystalList[_crystalList.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, _player.transform.position, Quaternion.identity);
                _crystalList.Remove(crystalToSpawn);
                newCrystal.GetComponent<SkillCrystalController>().
                    SetupCrystal(_crystalDuration, _canExplode, _canMoveToEnemy, _moveSpeed, FindClosestEnemy(newCrystal.transform), _damage);

                if (_crystalList.Count <= 0)
                {
                    _multiCrystalTimer = _cooldownOfMultiCrystal;
                    RefillCrystal();
                }
                return true;
            }
        }
        return false;
    }

    private void RefillCrystal()
    {
        int amountToAdd = _amountOfStack - _crystalList.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            _crystalList.Add(_crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if (_multiCrystalTimer > 0f)
            return;

        _multiCrystalTimer = _cooldownOfMultiCrystal;
        RefillCrystal();
    }
}