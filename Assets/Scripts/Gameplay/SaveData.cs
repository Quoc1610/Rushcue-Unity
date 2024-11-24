using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData 
{
    public int defaultCoins = 100;
    public int defaultSpeed = 5;
    public int defaultPrice = 100;
    public bool isFirstTime = true;
    public int coins;
    public int speedLvl;
    public SaveData()
    {
        coins = defaultCoins;
        speedLvl = defaultSpeed;
    }
    public SaveData(SaveData data)
    {
        coins = data.coins;
        speedLvl = data.speedLvl;
        isFirstTime = data.isFirstTime;
    }
}
