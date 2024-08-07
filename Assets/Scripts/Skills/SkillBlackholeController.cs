using System.Collections.Generic;
using UnityEngine;

public class SkillBlackholeController : MonoBehaviour
{
    [SerializeField] private GameObject _hotkeyPrefab;
    [SerializeField] private List<KeyCode> _hotkeyList = new();
    private bool _canCreateHotkeys = true;

    private float _maxSize;
    private float _growSpeed;
    private bool _canGrow = true;
    private float _shrinkSpeed;
    private bool _canShrink;
    private float _blackholeTimer;
    private bool _playerCanDisappear = true;

    private int _attackCount = 4;
    private float _cloneAttackCooldown = 0.3f;
    private float _cloneAttackTimer;
    private bool _cloneAttackReleased;

    private List<Transform> _targetList = new();
    private List<GameObject> _createdHotkeyList = new();

    public bool PlayerCanExit { get; private set; }


    internal void SetupBlackhole(float maxSize, float growSpeed, float shrinkSpeed, int attackAmount, float cloneAttackCD, float blackholeDuration)
    {
        _maxSize = maxSize;
        _growSpeed = growSpeed;
        _shrinkSpeed = shrinkSpeed;
        _attackCount = attackAmount;
        _cloneAttackCooldown = cloneAttackCD;
        _blackholeTimer = blackholeDuration;

        if (SkillManager.Instance.GetSkillClone().GetCrystalInsteadClone())
            _playerCanDisappear = false;
    }
    private void Update()
    {
        _cloneAttackTimer -= Time.deltaTime;
        _blackholeTimer -= Time.deltaTime;

        if (_blackholeTimer < 0)
        {
            _blackholeTimer = Mathf.Infinity;

            if (_targetList.Count > 0)
            {
                ReleaseCloneAttack();
            }
            else
            {
                FinishBlackholeAbility();
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReleaseCloneAttack();
        }

        CloneAttackLogic();

        if (_canGrow && !_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(_maxSize, _maxSize), _growSpeed * Time.deltaTime);
        }
        if (_canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), _shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }
    private void ReleaseCloneAttack()
    {
        if (_targetList.Count <= 0)
            return;

        _cloneAttackReleased = true;
        _canCreateHotkeys = false;
        DestroyHotkeys();


        if (_playerCanDisappear)
        {
            _playerCanDisappear = false;
            PlayerManager.Instance.Player.MakeTransparent(true);
        }
    }
    private void CloneAttackLogic()
    {
        if (_cloneAttackTimer <= 0 && _cloneAttackReleased && _attackCount > 0)
        {
            _cloneAttackTimer = _cloneAttackCooldown;

            int randomIndex = Random.Range(0, _targetList.Count);

            float xOffset = 1;
            if (Random.Range(0, 100) > 50)
                xOffset = 1;
            else
                xOffset = -1;

            if (SkillManager.Instance.GetSkillClone().GetCrystalInsteadClone())
            {
                SkillManager.Instance.GetSkillCrystal().CreateCrystal();
                SkillManager.Instance.GetSkillCrystal().CurrentCrystalChooseRandomEnemy();
            }
            else
            {
                SkillManager.Instance.GetSkillClone().CreateClone(_targetList[randomIndex], new Vector3(xOffset, 0, 0));
            }

            _attackCount--;
            if (_attackCount <= 0)
            {
                Invoke("FinishBlackholeAbility", 0.7f);
            }
        }
    }
    private void FinishBlackholeAbility()
    {
        PlayerCanExit = true;
        _canShrink = true;
        _cloneAttackReleased = false;
        DestroyHotkeys();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
            collision.GetComponent<Enemy>().FreezeTime(false);
    }
    private void CreateHotkey(Collider2D collision)
    {
        if (_hotkeyList.Count <= 0)
        {
            Debug.LogWarning("There is no hotkey in list");
            return;
        }

        if (!_canCreateHotkeys)
            return;

        KeyCode choosenKey = _hotkeyList[Random.Range(0, _hotkeyList.Count)];
        _hotkeyList.Remove(choosenKey);

        GameObject newHotkey = Instantiate(_hotkeyPrefab, collision.transform.position + new Vector3(0, 1.5f), Quaternion.identity);
        SkillBlackholeHotkey newBlackholeHotkey = newHotkey.GetComponent<SkillBlackholeHotkey>();
        newBlackholeHotkey.SetupHotKey(choosenKey, collision.transform, this);
        _createdHotkeyList.Add(newHotkey);
    }
    public void AddEnemyToList(Transform enemyTransform)
    {
        _targetList.Add(enemyTransform);
    }
    private void DestroyHotkeys()
    {
        if (_createdHotkeyList.Count <= 0)
            return;

        for (int i = 0; i < _createdHotkeyList.Count; i++)
        {
            Destroy(_createdHotkeyList[i]);
        }
    }

}