using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager Instance;

    [SerializeField] private Transform _checkpointHolder;
    [SerializeField] private List<Checkpoint> _checkpointList = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        Instance = this;
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

        foreach (Checkpoint checkpoint in _checkpointList)
        {
            if (gameData.ClosestCheckpointID == checkpoint.CheckpointID)
            {
                PlayerManager.Instance.Player.transform.position = checkpoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
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
            float distance = Vector2.Distance(PlayerManager.Instance.Player.transform.position, checkpoint.transform.position);
            if (distance < closestDistance && checkpoint.Activation)
            {
                closestDistance = distance;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
}