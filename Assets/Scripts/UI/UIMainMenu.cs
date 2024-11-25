using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private GameObject goCanBuy;

    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSpeed;

    public void OnSetUp()
    {
        
        txtCoin.text =UIManager.Instance().saveData.coins.ToString();
        txtPrice.text = UIManager.Instance().saveData.defaultPrice.ToString();
        CheckIfCanBuy();
        btnPlay.onClick.AddListener(OnPlay_Clicked);
        btnSpeed.onClick.AddListener(OnSpeed_Clicked);

    }
    public void OnPlay_Clicked()
    {
        UIManager.Instance().OnScene("GamePlay");
        
        UIManager.Instance().uiGameplay.gameObject.SetActive(true);
        
        UIManager.Instance().OnPause(0);
        this.gameObject.SetActive(false);


    }
    public void CheckIfCanBuy()
    {
        if (UIManager.Instance().saveData.coins >= UIManager.Instance().saveData.defaultPrice)
        {
            btnSpeed.interactable = true;
            goCanBuy.SetActive(true);
        }
        else
        {
            btnSpeed.interactable = false;
            goCanBuy.SetActive(false);
        }
    }
    public void OnSpeed_Clicked()
    {
       if(UIManager.Instance().saveData.coins >= UIManager.Instance().saveData.defaultPrice)
        {
            UIManager.Instance().saveData.coins -= UIManager.Instance().saveData.defaultPrice;
            UIManager.Instance().saveData.speedLvl++;
            txtCoin.text = UIManager.Instance().saveData.coins.ToString();
            SaveManager.SaveData(UIManager.Instance().saveData);
            CheckIfCanBuy();
        }
    }
}
