using System.Collections;
using UnityEngine;

public class SkillClone : Skill
{
    [SerializeField] private GameObject _clonePrefab;
    [SerializeField] private float _cloneDuration;
    [Space]
    [SerializeField] private bool _canAttack;

    [SerializeField] private bool _canCreateCloneOnDashStart;
    [SerializeField] private bool _canCreateCloneOnDashOver;
    [SerializeField] private bool _canCreateCloneOnCounterAttack;

    [Header("Duplication")]
    [SerializeField] private bool _canDuplicateClone;
    [SerializeField] private float _duplicationChance;

    [Header("Crystal instead of clone")]
    [SerializeField] private bool _crystalInsteadOfClone;



    internal void CreateClone(Transform clonePosition, Vector3 offset)
    {
        if (_crystalInsteadOfClone)
        {
            SkillManager.Instance.GetSkillCrystal().CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(_clonePrefab);
        newClone.GetComponent<SkillCloneController>().SetupClone
            (clonePosition, _cloneDuration, _canAttack, offset, FindClosestEnemy(clonePosition.transform), _canDuplicateClone, _duplicationChance);
    }

    public void CreateCloneOnDashBegun()
    {
        if (_canCreateCloneOnDashStart)
        {
            CreateClone(_player.transform, Vector3.zero);
        }
    }
    public void CreateCloneOnDashOver()
    {
        if (_canCreateCloneOnDashOver)
        {
            CreateClone(_player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnCounterAttack(Transform enemyTransform, float delaySecond)
    {
        if (_canCreateCloneOnCounterAttack)
        {
            StartCoroutine(IE_CreateCloneWithDelay(enemyTransform, new Vector3(1 * _player.GetFacingDirection(), 0), delaySecond));
        }
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
}