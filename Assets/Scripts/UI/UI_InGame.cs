using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private Slider _slider;

    [SerializeField] private Image _dashImage;
    [SerializeField] private Image _parryImage;
    [SerializeField] private Image _crystalImage;
    [SerializeField] private Image _swordImage;
    [SerializeField] private Image _blackholeImage;
    [SerializeField] private Image _potionImage;

    [Header("Souls Info")]
    [SerializeField] private TextMeshProUGUI _currentSoulsText;
    [SerializeField] private float _soulsAmount;
    [SerializeField] private float _increaseRate = 100;


    private SkillManager _skillManager;

    private void Start()
    {
        if (_playerStats != null)
            _playerStats.CharacterStats_OnHealthChanged += UpdateHealthUI;

        _skillManager = SkillManager.Instance;
    }
    private void OnDestroy()
    {
        if (_playerStats != null)
            _playerStats.CharacterStats_OnHealthChanged -= UpdateHealthUI;
    }
    private void Update()
    {
        UpdateSoulsUI();

        if (Input.GetKeyDown(KeyCode.LeftShift) && _skillManager.GetSkillDash().DashUnlocked)
            SetCooldownOf(_dashImage);

        //if (Input.GetKeyDown(KeyCode.Q) && _skillManager.GetSkillParry().ParryUnlocked)
        //SetCooldownOf(_parryImage);

        if (Input.GetKeyDown(KeyCode.F) && _skillManager.GetSkillCrystal().CrystalUnlocked)
            SetCooldownOf(_crystalImage);

        //if (Input.GetKeyDown(KeyCode.Q) && _skillManager.GetSkillSword().SwordUnlocked)
        //    SetCooldownOf(_swordImage);

        if (Input.GetKeyDown(KeyCode.R) && _skillManager.GetSkillBlackhole().BlackholeUnlocked)
            SetCooldownOf(_blackholeImage);

        if (Input.GetKeyDown(KeyCode.E) && Inventory.Instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(_potionImage);

        if (Input.GetKeyDown(KeyCode.V))
            PlayerManager.Instance.Currency += 1000;

        if (Input.GetKeyDown(KeyCode.N))
            PlayerManager.Instance.Currency = Mathf.Max(PlayerManager.Instance.Currency - 1000, 0);

        CheckCooldownOf(_dashImage, _skillManager.GetSkillDash().GetCooldown());
        CheckCooldownOf(_parryImage, _skillManager.GetSkillParry().GetCooldown());
        CheckCooldownOf(_crystalImage, _skillManager.GetSkillCrystal().GetCooldown());
        //CheckCooldownOf(_swordImage, _skillManager.GetSkillSword().GetCooldown());
        CheckCooldownOf(_blackholeImage, _skillManager.GetSkillBlackhole().GetCooldown());
        CheckSwordCooldown();

        if (Inventory.Instance.GetEquipment(EquipmentType.Flask) != null)
            CheckCooldownOf(_potionImage, Inventory.Instance.GetFlaskCooldown());

    }
    private void UpdateSoulsUI()
    {
        _increaseRate = Mathf.Max(Mathf.Abs(PlayerManager.Instance.Currency - _soulsAmount), 100);
        if (_soulsAmount != PlayerManager.Instance.Currency)
        {
            _soulsAmount = Mathf.RoundToInt(Mathf.MoveTowards(_soulsAmount, PlayerManager.Instance.Currency, Time.deltaTime * _increaseRate));
        }
        _currentSoulsText.text = _soulsAmount.ToString();
        //Debug.Log($"increase rate : {_increaseRate} \n souls amount : {_soulsAmount} \n manager currency : {PlayerManager.Instance.Currency}");
    }
    private void UpdateHealthUI()
    {
        _slider.value = _playerStats.GetCurrentHealthPercentage();
    }
    private void SetCooldownOf(Image image)
    {
        if (image.fillAmount <= 0)
        {
            image.fillAmount = 1;
        }
    }
    private void CheckCooldownOf(Image image, float cooldown)
    {
        if (image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }
    private void CheckSwordCooldown()
    {
        if (PlayerManager.Instance.Player.GetSword() != null)
        {
            _swordImage.fillAmount = 1;
        }
        else
        {
            _swordImage.fillAmount = 0;
        }
    }
}
