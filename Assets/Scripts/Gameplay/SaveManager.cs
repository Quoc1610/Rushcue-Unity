using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void SaveData(SaveData data)
    {
        string path = Application.persistentDataPath + "/save.json";
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(path, jsonData);
    }
    public static void ClearData()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }
    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/save.json";
        if (File.Exists(path))
        {
            string jsonData = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(jsonData);
        }
        else
        {
            return new SaveData(); // Default data
        }
    }

}
