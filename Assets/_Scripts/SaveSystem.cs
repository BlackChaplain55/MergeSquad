using System;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveSystem
{
    public static SaveSystem Instance {
        get
        {
            if (_instance == null)
                _instance = new SaveSystem();
            return _instance;
        }
        private set { _instance = value; }
    }
    private static SaveSystem _instance;

    public bool TryLoadGame(string saveName, out GameProgress gameProgress)
    {
        //Debug.Log("Persistent data path : " + Application.persistentDataPath);
        string savePath = $"{Application.persistentDataPath}/{saveName}.json";
        if (File.Exists(savePath))
        {
            string saveData = File.ReadAllText(savePath);
            try
            {
                //GameProgress = JsonConvert.DeserializeObject<GameProgress>(saveData);
                gameProgress = JsonUtility.FromJson<GameProgress>(saveData);
                return true;
            }
            catch (Exception e)
            {
                Debug.Log(savePath);
                Debug.LogError("Saved data in file cannot be converted to GameProgress : " + e);
            }
            gameProgress = default;
            return false;
        }
        else
        {
            gameProgress = default;
            return false;
        }
    }

    public bool TrySaveGame(string saveName, GameProgress gameProgress)
    {
        string savePath = $"{Application.persistentDataPath}/{saveName}.json";
        using (var fileStream = new FileStream(savePath, FileMode.OpenOrCreate))
        {
            //byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_gameProgress));
            byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(gameProgress));
            fileStream.Write(data);
        }
        return true;
    }
}
