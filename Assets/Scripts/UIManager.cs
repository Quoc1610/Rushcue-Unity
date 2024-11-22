using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance { get; private set; }
    private void Awake() {
         if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
    }
    private void Start() {
        
    }
    public void OnLoadGameScene()
    {
        
    }
}