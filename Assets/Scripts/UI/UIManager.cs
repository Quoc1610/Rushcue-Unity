using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public UILoading uiLoading;
    public UIMainMenu uiMainMenu;
    public SaveData saveData;
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
    }
    private void Start()
    {
        saveData = SaveManager.LoadData();
        
        uiLoading.OnSetUp();
        uiMainMenu.OnSetUp();
        uiMainMenu.gameObject.SetActive(false);
    }
    public void OnLevelWasLoaded(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void OnClose_Clicked()
    {
        this.gameObject.SetActive(false);
    }

}
