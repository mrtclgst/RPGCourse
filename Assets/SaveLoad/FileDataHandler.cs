using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirectionPath = string.Empty;
    private string _dataFileName = string.Empty;

    public FileDataHandler(string dataDirectionPath, string dataFileName)
    {
        this._dataDirectionPath = dataDirectionPath;
        this._dataFileName = dataFileName;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(_dataDirectionPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);
            using (FileStream stream = new(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch (Exception exception)
        {
            Debug.LogError("Error on trying to save data to file " + fullPath + "\n" + exception.Message);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(_dataDirectionPath, _dataFileName);
        GameData loadData = new();

        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = string.Empty;
                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception exception)
            {
                Debug.LogError("Data wasn't loaded " + exception.Message + "\n" + "path: " + fullPath);
            }
        }
        return loadData;
    }
}
