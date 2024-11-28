using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private TextMeshProUGUI txtPrice;
    [SerializeField] private TextMeshProUGUI txtLvlSpeed;
    [SerializeField] private GameObject goCanBuy;
    [SerializeField] private List<Button> lsBtnLvl= new List<Button>();
    [SerializeField] private TextMeshProUGUI txtLvl;
    public int lvlMap;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSpeed;
    public int price;

    public void OnSetUp()
    {
        ChangeText();
        CheckIfCanBuy();
        lvlMap = 1;
        txtLvl.text = "Map " + lvlMap;
        btnPlay.onClick.AddListener(OnPlay_Clicked);
        btnSpeed.onClick.AddListener(OnSpeed_Clicked);

    }
    public void OnLvl_Clicked(int index)
    {
        if (index == 0)
        {
            if (lvlMap > 1)
            {
                lvlMap--;
                txtLvl.text = "Map " + lvlMap;
            }
        }
        else
        {
            if (lvlMap < 10)
            {
                lvlMap++;
                txtLvl.text = "Map " + lvlMap;
            }
        }
    }
    public void ChangeText()
    {
        price = UIManager.Instance().saveData.defaultPrice * UIManager.Instance().saveData.speedLvl;
        txtCoin.text = UIManager.Instance().saveData.coins.ToString();
        txtPrice.text = price.ToString();
        txtLvlSpeed.text = (.18f * 3 + (.18f * UIManager.Instance().saveData.speedLvl) / 2).ToString();
    }
    public void OnPlay_Clicked()
    {
        UIManager.Instance().OnScene("GamePlay");
        UIManager.Instance().uiGameplay.gameObject.SetActive(true);
        UIManager.Instance().uiGameplay.OnSetUp();
        UIManager.Instance().OnPause(0);
        
        this.gameObject.SetActive(false);


    }
    public void CheckIfCanBuy()
    {
        if (UIManager.Instance().saveData.coins >= price)
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
       if(UIManager.Instance().saveData.coins >= price)
        {
            UIManager.Instance().saveData.coins -= price;
            UIManager.Instance().saveData.speedLvl++;
            ChangeText();
            SaveManager.SaveData(UIManager.Instance().saveData);
            CheckIfCanBuy();
        }
    }
}
