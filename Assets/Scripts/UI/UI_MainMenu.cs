using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{
    [SerializeField] private string _sceneName = "MainScene";
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private UI_FadeScreen _fadeScreen;

    private void Start()
    {
        if (!SaveManager.Instance.HasSavedData())
        {
            _continueButton.SetActive(false);
        }
    }
    public void ContinueGame()
    {
        StartCoroutine(IE_LoadSceneWithFadeEffect(1.5f));
    }
    public void NewGame()
    {
        SaveManager.Instance.DeleteSavedData();
        StartCoroutine(IE_LoadSceneWithFadeEffect(1.5f));
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private IEnumerator IE_LoadSceneWithFadeEffect(float seconds)
    {
        _fadeScreen.FadeOut();
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(_sceneName);
    }
}
