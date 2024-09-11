using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] private TextMeshProUGUI _currentSoulsText;

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
        if (Input.GetKeyDown(KeyCode.LeftShift) && _skillManager.GetSkillDash().DashUnlocked)
            SetCooldownOf(_dashImage);

        if (Input.GetKeyDown(KeyCode.Q) && _skillManager.GetSkillParry().ParryUnlocked)
            SetCooldownOf(_parryImage);

        if (Input.GetKeyDown(KeyCode.F) && _skillManager.GetSkillCrystal().CrystalUnlocked)
            SetCooldownOf(_crystalImage);

        if (Input.GetKeyDown(KeyCode.Mouse1) && _skillManager.GetSkillSword().SwordUnlocked)
            SetCooldownOf(_swordImage);

        if (Input.GetKeyDown(KeyCode.R) && _skillManager.GetSkillBlackhole().BlackholeUnlocked)
            SetCooldownOf(_blackholeImage);

        if (Input.GetKeyDown(KeyCode.Keypad1) && Inventory.Instance.GetEquipment(EquipmentType.Flask) != null)
            SetCooldownOf(_potionImage);

        CheckCooldownOf(_dashImage, _skillManager.GetSkillDash().GetCooldown());
        CheckCooldownOf(_parryImage, _skillManager.GetSkillParry().GetCooldown());
        CheckCooldownOf(_crystalImage, _skillManager.GetSkillCrystal().GetCooldown());
        CheckCooldownOf(_swordImage, _skillManager.GetSkillSword().GetCooldown());
        CheckCooldownOf(_blackholeImage, _skillManager.GetSkillBlackhole().GetCooldown());


        if (Inventory.Instance.GetEquipment(EquipmentType.Flask) != null)
            CheckCooldownOf(_potionImage, Inventory.Instance.GetFlaskCooldown());

        _currentSoulsText.text = PlayerManager.Instance.CurrentCurrencyAmount().ToString("#,#");
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
}
