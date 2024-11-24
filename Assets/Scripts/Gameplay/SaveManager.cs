using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveManager
{
    public static void SaveData(SaveData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData saveData = new SaveData(data);
        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveData LoadData()
    {
        string path = Application.persistentDataPath + "/save.data";
        if (File.Exists(path)) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            SaveData defaultData = new SaveData();
            return defaultData;
        }
    }

    internal static void SaveData(GameplayManager gameplayManager)
    {
        throw new NotImplementedException();
    }
}
