using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public int coins = 0;
    public int speedlvl = 0;

    public void SaveData()
    {
        SaveManager.SaveData(this);
    }
    public void LoadData()
    {
        SaveData data = SaveManager.LoadData();
        coins = data.coins;
        speedlvl = data.speedLvl;
    }
}
