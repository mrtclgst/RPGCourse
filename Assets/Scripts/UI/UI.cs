using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private GameObject _skillTreeUI;
    [SerializeField] private GameObject _craftUI;
    [SerializeField] private GameObject _optionsUI;
    [SerializeField] private GameObject _inGameUI;


    public UI_ItemTooltip ItemTooltip;
    public UI_StatTooltip StatTooltip;
    public UI_CraftWindow CraftWindow;
    public UI_SkillTooltip SkillTooltip;

    private void Awake()
    {
        SwitchTo(_skillTreeUI);
    }

    private void Start()
    {
        SwitchTo(_inGameUI);
        ItemTooltip.gameObject.SetActive(false);
        StatTooltip.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(_characterUI);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SwitchWithKeyTo(_craftUI);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchWithKeyTo(_skillTreeUI);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(_optionsUI);
        }
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; transform.childCount > i; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
            menu.SetActive(true);
    }
    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForInGameUI();
            return;
        }
        SwitchTo(menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)
                return;
        }

        SwitchTo(_inGameUI);
    }
}