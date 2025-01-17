using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillSword : Skill
{
    public SwordType SwordType = SwordType.Regular;
    [SerializeField] private int _damage;

    [Header("Bounce Info")]
    [SerializeField] private UI_SkillTreeSlot _bounceUnlockButton;
    [SerializeField] private int _bounceAmount;
    [SerializeField] private float _bounceGravity;
    [SerializeField] private float _bounceSpeed;

    [Header("Pierce Info")]
    [SerializeField] private UI_SkillTreeSlot _pierceUnlockButton;
    [SerializeField] private int _pierceAmount;
    [SerializeField] private float _pierceGravity;

    [Header("Spin Info")]
    [SerializeField] private UI_SkillTreeSlot _spinUnlockButton;
    [SerializeField] private float _damageCooldown = 0.35f;
    [SerializeField] private float _maxTravelDistance;
    [SerializeField] private float _spinDuration;
    [SerializeField] private float _spinGravity;


    [Header("Skill Info")]
    [SerializeField] private UI_SkillTreeSlot _swordUnlockButton;
    public bool SwordUnlocked { get; private set; }
    [SerializeField] private GameObject _swordPrefab;
    [SerializeField] private Vector2 _launchForce;
    [SerializeField] private float _swordGravityScale;
    [SerializeField] private float _freezeTimeDuration;
    [SerializeField] private float _returnSpeed;

    private Vector2 _finalDirection;

    [Header("Aim Dots")]
    [SerializeField] private int _numberOfDots;
    [SerializeField] private float _spaceBetweenDots;
    [SerializeField] private GameObject _dotPrefab;
    [SerializeField] private Transform _dotParent;

    private GameObject[] _dotArray;

    [Header("Passive Skills")]
    [SerializeField] private UI_SkillTreeSlot _timeStopUnlockButton;
    public bool TimeStopUnlocked { get; private set; }
    [SerializeField] private UI_SkillTreeSlot _vulnurabilityUnlockButton;
    public bool VulnurabilityUnlocked { get; private set; }

    protected override void Start()
    {
        base.Start();
        GenerateDots();
        SetupGravity();
        _swordUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSword);
        _bounceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockBounceSword);
        _pierceUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockPierceSword);
        _spinUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockSpinSword);
        _timeStopUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockTimeStop);
        _vulnurabilityUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockVulnurablity);
    }
    protected override void CheckUnlock()
    {
        UnlockSword();
        UnlockBounceSword();
        UnlockPierceSword();
        UnlockTimeStop();
        UnlockVulnurablity();
    }
    private void SetupGravity()
    {
        if (SwordType == SwordType.Bouncing)
        {
            _swordGravityScale = _bounceGravity;
        }
        else if (SwordType == SwordType.Piercing)
        {
            _swordGravityScale = _pierceGravity;
        }
        else if (SwordType == SwordType.Spinning)
        {
            _swordGravityScale = _spinGravity;
        }
    }
    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Vector2 targetDir = AimDirection().normalized;
            _finalDirection =
                new Vector2(targetDir.x * _launchForce.x, targetDir.y * _launchForce.y);
        }
        for (int i = 0; i < _dotArray.Length; i++)
        {
            _dotArray[i].transform.position = DotPosition(i * _spaceBetweenDots);
        }
    }
    public void CreateSword()
    {
        GameObject newSword = Instantiate(_swordPrefab, _player.transform.position, transform.rotation);
        SkillSwordController skillSwordController = newSword.GetComponent<SkillSwordController>();

        if (SwordType == SwordType.Bouncing)
        {
            skillSwordController.SetupBounce(true, _bounceAmount, _bounceSpeed);
        }
        else if (SwordType == SwordType.Piercing)
        {
            skillSwordController.SetupPierce(_pierceAmount);
        }
        else if (SwordType == SwordType.Spinning)
        {
            skillSwordController.SetupSpin(true, _maxTravelDistance, _spinDuration, _damageCooldown);
        }

        skillSwordController.SetupSword(_finalDirection, _swordGravityScale, _player, _freezeTimeDuration, _returnSpeed, _damage);
        _player.AssignNewSword(newSword);
        DotsActive(false);
    }

    #region Unlock Region
    private void UnlockTimeStop()
    {
        if (_timeStopUnlockButton.Unlocked)
            TimeStopUnlocked = true;
    }
    private void UnlockVulnurablity()
    {
        if (_vulnurabilityUnlockButton.Unlocked)
            VulnurabilityUnlocked = true;
    }
    private void UnlockSword()
    {
        if (_swordUnlockButton.Unlocked)
        {
            SwordType = SwordType.Regular;
            SwordUnlocked = true;
        }
    }
    private void UnlockBounceSword()
    {
        if (_bounceUnlockButton.Unlocked)
            SwordType = SwordType.Bouncing;
    }
    private void UnlockPierceSword()
    {
        if (_pierceUnlockButton.Unlocked)
            SwordType = SwordType.Piercing;
    }
    private void UnlockSpinSword()
    {
        if (_spinUnlockButton.Unlocked)
            SwordType = SwordType.Spinning;
    }
    #endregion

    #region AimRegion
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = _player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }
    public void DotsActive(bool isActive)
    {
        for (int i = 0; i < _dotArray.Length; i++)
        {
            _dotArray[i].SetActive(isActive);
        }
    }
    private void GenerateDots()
    {
        _dotArray = new GameObject[_numberOfDots];
        for (int i = 0; i < _numberOfDots; i++)
        {
            _dotArray[i] = Instantiate(_dotPrefab, _player.transform.position, Quaternion.identity, _dotParent);
            _dotArray[i].SetActive(false);
        }
    }
    private Vector2 DotPosition(float t)
    {
        Vector2 direction = AimDirection().normalized;
        Vector2 position = (Vector2)_player.transform.position +
            new Vector2(direction.x * _launchForce.x, direction.y * _launchForce.y) * t + 0.5f * (Physics2D.gravity * _swordGravityScale) * (t * t);
        return position;
    }

    
    #endregion
}

public enum SwordType
{
    Regular,
    Bouncing,
    Piercing,
    Spinning,
}