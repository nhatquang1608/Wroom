using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadData : MonoBehaviour
{
    public static SaveLoadData Instance;
    public int vehicleID;
    public int levelIndex;
    private string filePath;
    public PlayerInfo playerInfo;

    public void Awake()
    {
        filePath = Application.persistentDataPath + "/data.json";

        if (!File.Exists(filePath))
        {
            SaveData();
        }
        else
        {
            LoadData();
        }

        if (Instance != null) 
        {
            DestroyImmediate(gameObject);
        } 
        else 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SaveData()
    {
        string jsonData = JsonUtility.ToJson(playerInfo);
        File.WriteAllText(filePath, jsonData);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
            JsonUtility.FromJsonOverwrite(jsonData, playerInfo);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }
}
