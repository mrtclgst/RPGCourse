using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private GameData _gameData;
    private List<ISaveManager> _saveManagerList;
    private FileDataHandler _fileDataHandler;
    [SerializeField] private string _fileName;

    private void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        else
            Instance = this;
    }
    private void OnEnable()
    {
        _fileDataHandler = new(Application.persistentDataPath, _fileName);
        _saveManagerList = FindAllSaveManagers();
        LoadGame();
    }
    public void NewGame()
    {
        _gameData = new GameData();
    }
    public void LoadGame()
    {
        _gameData = _fileDataHandler.Load();

        if (_gameData == null)
        {
            Debug.Log("No saved data found!");
            NewGame();
        }

        foreach (ISaveManager saveManager in _saveManagerList)
        {
            saveManager.LoadData(_gameData);
        }
    }
    public void SaveGame()
    {
        foreach (ISaveManager saveManager in _saveManagerList)
        {
            saveManager.SaveData(ref _gameData);
        }

        _fileDataHandler.Save(_gameData);
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }
}
