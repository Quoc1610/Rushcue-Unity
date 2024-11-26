using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public UILoading uiLoading;
    public UIMainMenu uiMainMenu;
    public UIResult uiResult;
    public UIGameplay uiGameplay;
    public SaveData saveData;
    public int coinAnimal, coinDisRun;
    
    public static UIManager Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.FindObjectOfType<UIManager>();
        }
        return _instance;
    }
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        SaveManager.ClearData();
    }
    private void Start()
    {
        saveData = SaveManager.LoadData();
        Debug.Log(saveData.coins);
       
        coinAnimal = 0;
        coinDisRun = 0;
        uiLoading.OnSetUp();
        uiMainMenu.OnSetUp();
        uiMainMenu.gameObject.SetActive(false);
    }
    public void OnScene(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void OnReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public int CalculateCoin()
    {
        
        coinAnimal = AllManager.Instance().lsCaughtAnimal.Count * 50;
        coinDisRun = (int)PlayerManager.Instance().disRun * 5;
        return coinAnimal + coinDisRun;
    }
    public void OnPause(int a)
    {
        if(a == 0)
        {
            Time.timeScale = 1;
        }
        else Time.timeScale = 0;
    }
    public void OnClose_Clicked()
    {
        this.gameObject.SetActive(false);
    }

}
