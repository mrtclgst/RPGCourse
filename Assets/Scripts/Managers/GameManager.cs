using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager Instance;

    private Transform _player;

    [SerializeField] private Transform _checkpointHolder;
    [SerializeField] private List<Checkpoint> _checkpointList = new();

    [Header("Lost Currency")]
    [SerializeField] private GameObject _lostCurrencyPrefab;
    public int LostCurrencyAmount;
    [SerializeField] private float _lostCurrencyXPos;
    [SerializeField] private float _lostCurrencyYPos;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        _player = PlayerManager.Instance.Player.transform;
    }
    public void RestartScene()
    {
        SaveManager.Instance.SaveGame();
        Invoke("LoadScene", 0.3f);
    }

    private void LoadScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData gameData)
    {
        LoadCheckpoints(gameData);
        //LoadLostCurrency(gameData);
        StartCoroutine(IE_LoadWithDelay(gameData));
    }

    private void LoadCheckpoints(GameData gameData)
    {
        foreach (KeyValuePair<string, bool> pair in gameData.Checkpoints)
        {
            foreach (Checkpoint checkpoint in _checkpointList)
            {
                if (checkpoint.CheckpointID == pair.Key && pair.Value == true)
                {
                    checkpoint.ActivateCheckpoint();
                }
            }
        }
    }

    private void LoadLostCurrency(GameData gameData)
    {
        LostCurrencyAmount = gameData.LostCurrencyAmount;
        _lostCurrencyXPos = gameData.LostCurrencyX;
        _lostCurrencyYPos = gameData.LostCurrencyY;

        if (LostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(_lostCurrencyPrefab, new Vector3(_lostCurrencyXPos, _lostCurrencyYPos), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().Currency = LostCurrencyAmount;
        }

        LostCurrencyAmount = 0;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.LostCurrencyAmount = LostCurrencyAmount;
        gameData.LostCurrencyX = _player.position.x;
        gameData.LostCurrencyY = _player.position.y;

        gameData.ClosestCheckpointID = FindClosestCheckpoint()?.CheckpointID;
        gameData.Checkpoints.Clear();

        foreach (Checkpoint checkpoint in _checkpointList)
        {
            gameData.Checkpoints.Add(checkpoint.CheckpointID, checkpoint.Activation);
        }
    }

    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (Checkpoint checkpoint in _checkpointList)
        {
            float distance = Vector2.Distance(_player.position, checkpoint.transform.position);
            if (distance < closestDistance && checkpoint.Activation)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }

    private IEnumerator IE_LoadWithDelay(GameData gameData)
    {
        yield return new WaitForSeconds(0.1f);
        LoadLostCurrency(gameData);

        foreach (Checkpoint checkpoint in _checkpointList)
        {
            if (gameData.ClosestCheckpointID == checkpoint.CheckpointID)
            {
                _player.position = checkpoint.transform.position;
            }
        }
    }
}