using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCrystal : Skill
{
    [SerializeField] private GameObject _crystalPrefab;
    [SerializeField] private float _crystalDuration;
    [SerializeField] private int _damage;

    [Header("Crystal Mirage")]
    [SerializeField] private UI_SkillTreeSlot _spawnCloneInsteadButton;
    [SerializeField] private bool _canSpawnClone;

    [Header("Simple Crystal")]
    [SerializeField] private UI_SkillTreeSlot _unlockCrystalButton;
    public bool CrystalUnlocked;

    [Header("Explosive Crystal")]
    [SerializeField] private UI_SkillTreeSlot _unlockExplosiveCrystalButton;
    [SerializeField] private bool _canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private UI_SkillTreeSlot _unlockMovingCrystalButton;
    [SerializeField] private bool _canMoveToEnemy;
    [SerializeField] private float _moveSpeed;

    [Header("Multi Stacking Crystals")]
    [SerializeField] private UI_SkillTreeSlot _unlockMultiCrystalButton;
    [SerializeField] private bool _canUseMultiStackCrystal;
    [SerializeField] private int _amountOfStack;
    [SerializeField] private float _cooldownOfMultiCrystal;
    [SerializeField] private float _useTimeWindow;
    private float _multiCrystalTimer;
    [SerializeField] private List<GameObject> _crystalList = new();

    private GameObject _currentCrystal;

    protected override void Start()
    {
        base.Start();
        _unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        _spawnCloneInsteadButton.GetComponent<Button>().onClick.AddListener(UnlockMirageCrystal);
        _unlockExplosiveCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockExplosiveCrystal);
        _unlockMovingCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMovingCrystal);
        _unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);
    }

    protected override void CheckUnlock()
    {
        UnlockCrystal();
        UnlockExplosiveCrystal();
        UnlockMirageCrystal();
        UnlockMovingCrystal();
        UnlockMultiCrystal();
    }

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
        //base.UseSkill();

        if (_canUseMultiStackCrystal)
        {
            if (CanUseMultiCrystal())
                return;

            return;
        }
        if (!_currentCrystal && CanUseSkill())
        {
            CreateCrystal();
            _cooldownTimer = _cooldown;
        }
        else if (_currentCrystal != null)
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
                    _cooldown = _cooldownOfMultiCrystal;
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

    #region Skill Unlocking Area
    private void UnlockCrystal()
    {
        if (_unlockCrystalButton.Unlocked)
            CrystalUnlocked = true;
    }
    private void UnlockMirageCrystal()
    {
        if (_spawnCloneInsteadButton.Unlocked)
            _canSpawnClone = true;
    }
    private void UnlockExplosiveCrystal()
    {
        if (_unlockExplosiveCrystalButton.Unlocked)
            _canExplode = true;
    }
    private void UnlockMovingCrystal()
    {
        if (_unlockMovingCrystalButton.Unlocked)
            _canMoveToEnemy = true;
    }
    private void UnlockMultiCrystal()
    {
        if (_unlockMultiCrystalButton.Unlocked)
            _canUseMultiStackCrystal = true;
    }
    #endregion
}