using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour, ISaveManager
{
    public UI_FadeScreen _fadeScreen;
    [SerializeField] private GameObject _youDiedText;
    [SerializeField] private GameObject _restartButton;
    [Space]
    [SerializeField] private GameObject _characterUI;
    [SerializeField] private GameObject _skillTreeUI;
    [SerializeField] private GameObject _craftUI;
    [SerializeField] private GameObject _optionsUI;
    [SerializeField] private GameObject _inGameUI;


    public UI_ItemTooltip ItemTooltip;
    public UI_StatTooltip StatTooltip;
    public UI_CraftWindow CraftWindow;
    public UI_SkillTooltip SkillTooltip;

    [SerializeField] private UI_VolumeSlider[] _volumeSettingsArray;

    private void Awake()
    {
        SwitchTo(_skillTreeUI);
    }
    private IEnumerator Start()
    {
        ItemTooltip.gameObject.SetActive(false);
        StatTooltip.gameObject.SetActive(false);
        _fadeScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        SwitchTo(_inGameUI);
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchWithKeyTo(_optionsUI);
        }
    }
    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; transform.childCount > i; i++)
        {
            bool fadeScreen = transform.GetChild(i).GetComponent<UI_FadeScreen>() != null;
            if (!fadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);
            AudioManager.Instance.PlaySFX(28, null);
        }

        if (GameManager.Instance != null)
        {
            if (menu == _inGameUI)
                GameManager.Instance.PauseGame(false);
            else
                GameManager.Instance.PauseGame(true);
        }
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
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<UI_FadeScreen>() == null)
                return;
        }

        SwitchTo(_inGameUI);
    }
    public void SwitchOnEndScreen()
    {
        StartCoroutine(IE_EndScreenCoroutine());
    }
    private IEnumerator IE_EndScreenCoroutine()
    {
        _fadeScreen.gameObject.SetActive(true);
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(1);
        _youDiedText.SetActive(true);
        yield return new WaitForSeconds(1);
        _restartButton.SetActive(true);
    }
    public void RestartGameButton()
    {
        GameManager.Instance.RestartScene();
    }
    public void LoadData(GameData gameData)
    {
        foreach (KeyValuePair<string, float> pair in gameData.VolumeSettings)
        {
            foreach (UI_VolumeSlider volumeSlider in _volumeSettingsArray)
            {
                if (pair.Key == volumeSlider.Parameter)
                {
                    volumeSlider.LoadSlider(pair.Value);
                }
            }
        }
    }
    public void SaveData(ref GameData gameData)
    {
        gameData.VolumeSettings.Clear();

        foreach (UI_VolumeSlider volumeSlider in _volumeSettingsArray)
        {
            gameData.VolumeSettings.Add(volumeSlider.Parameter, volumeSlider.Slider.value);
        }
    }
    public void EndGame()
    {
        Application.Quit();
    }
}