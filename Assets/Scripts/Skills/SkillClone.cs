using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SkillClone : Skill
{
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private float _cloneDuration;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackMultiplier;

    [Header("Clone Attack")]
    [SerializeField] private UI_SkillTreeSlot _cloneAttackUnlockButton;
    [SerializeField] private float _cloneAttackMultiplier;
    [SerializeField] private bool _canAttack;

    [Header("Aggressive Clone")]
    [SerializeField] private UI_SkillTreeSlot _aggressiveCloneUnlockButton;
    [SerializeField] private float _aggressiveCloneAttackMultiplier;
    public bool CanApplyOnHitEffect { get; private set; }

    [Header("Multi Clone")]
    [SerializeField] private UI_SkillTreeSlot _multipleCloneUnlockButton;
    [SerializeField] private float _multiCloneAttackMultiplier;
    [SerializeField] private bool _canDuplicateClone;
    [SerializeField] private float _duplicationChance;

    [Header("Crystal instead of clone")]
    [SerializeField] private UI_SkillTreeSlot _crystalInsteadUnlockButton;
    [SerializeField] private bool _crystalInsteadOfClone;


    protected override void Start()
    {
        base.Start();
        _cloneAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneAttack);
        _aggressiveCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockAggressiveClone);
        _multipleCloneUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockMultiClone);
        _crystalInsteadUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalInsteadOfClone);
    }

    internal void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (_crystalInsteadOfClone)
        {
            SkillManager.Instance.GetSkillCrystal().CreateCrystal();
            return;
        }

        int calculatedDamage = Mathf.RoundToInt(_damage * _attackMultiplier);

        GameObject newClone = Instantiate(_clonePrefab);
        newClone.GetComponent<SkillCloneController>().SetupClone
            (clonePosition, _cloneDuration, _canAttack, offset, FindClosestEnemy(clonePosition.transform), _canDuplicateClone, _duplicationChance, calculatedDamage);
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform, float delaySecond)
    {
        StartCoroutine(IE_CreateCloneWithDelay(enemyTransform, new Vector3(1 * _player.GetFacingDirection(), 0), delaySecond));
    }
    private IEnumerator IE_CreateCloneWithDelay(Transform targetTransform, Vector3 offset, float second)
    {
        yield return new WaitForSeconds(second);
        CreateClone(targetTransform, offset);
    }
    internal bool GetCrystalInsteadClone()
    {
        return _crystalInsteadOfClone;
    }

    #region Unlock Region

    private void UnlockCloneAttack()
    {
        if (_cloneAttackUnlockButton.Unlocked)
        {
            _canAttack = true;
            _attackMultiplier = _cloneAttackMultiplier;
        }
    }

    private void UnlockAggressiveClone()
    {
        if (_aggressiveCloneUnlockButton.Unlocked)
        {
            CanApplyOnHitEffect = true;
            _attackMultiplier = _aggressiveCloneAttackMultiplier;
        }
    }

    private void UnlockMultiClone()
    {
        if (_multipleCloneUnlockButton.Unlocked)
        {
            _canDuplicateClone = true;
            _attackMultiplier = _multiCloneAttackMultiplier;
        }
    }

    private void UnlockCrystalInsteadOfClone()
    {
        if (_crystalInsteadUnlockButton.Unlocked)
        {
            _crystalInsteadOfClone = true;
        }
    }

    #endregion
}