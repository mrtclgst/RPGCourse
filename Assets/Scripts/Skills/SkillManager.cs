using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [SerializeField] private SkillDash _skillDash;
    [SerializeField] private SkillClone _skillClone;
    [SerializeField] private SkillSword _skillSword;
    [SerializeField] private SkillBlackhole _skillBlackhole;
    [SerializeField] private SkillCrystal _skillCrystal;
    [SerializeField] private SkillParry _skillParry;
    [SerializeField] private SkillDodge _skillDodge;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }
    internal SkillDash GetSkillDash() { return _skillDash; }
    internal SkillClone GetSkillClone() { return _skillClone; }
    internal SkillSword GetSkillSword() { return _skillSword; }
    internal SkillBlackhole GetSkillBlackhole() { return _skillBlackhole; }
    internal SkillCrystal GetSkillCrystal() { return _skillCrystal; }
    internal SkillParry GetSkillParry() { return _skillParry; }
    internal SkillDodge GetSkillDodge() { return _skillDodge; }
}