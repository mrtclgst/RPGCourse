using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string _dataDirectionPath = string.Empty;
    private string _dataFileName = string.Empty;

    private bool _encryptData = false;
    private string _codeWord = "devMert";

    public FileDataHandler(string dataDirectionPath, string dataFileName, bool encrypty)
    {
        this._dataDirectionPath = dataDirectionPath;
        this._dataFileName = dataFileName;
        this._encryptData = encrypty;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(_dataDirectionPath, _dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToStore = JsonUtility.ToJson(gameData, true);
            if (_encryptData)
                dataToStore = EncryptDecrypt(dataToStore);

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

                if (_encryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                    Debug.Log("here");
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

    public void DeleteData()
    {
        string fullPath = Path.Combine(_dataDirectionPath, _dataFileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedData = string.Empty;
        for (int i = 0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ _codeWord[i % _codeWord.Length]);
        }
        return modifiedData;
    }
}
