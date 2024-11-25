using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIResult : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtResult;
    [SerializeField] private TextMeshProUGUI txtCoins;
    [SerializeField] private Button btnDone;

    public void OnSetUp(int result,int coin)
    {
        UIManager.Instance().uiGameplay.joystick.gameObject.SetActive(false);
        if (result == 1)
        {
            txtResult.text = "You Win!";
            txtCoins.text =coin.ToString();
        }
        else
        {
            txtResult.text = "You Lose!";
            txtCoins.text = coin.ToString();

        }
    }
    public void OnDone_Clicked()
    {
        UIManager.Instance().OnPause(0);
        //UIManager.Instance().OnScene("UI");
        UIManager.Instance().uiMainMenu.gameObject.SetActive(true);
        UIManager.Instance().uiMainMenu.OnSetUp();
        UIManager.Instance().OnReloadScene();
        UIManager.Instance().saveData.coins += int.Parse(txtCoins.text);
        SaveManager.SaveData(UIManager.Instance().saveData);
        this.gameObject.SetActive(false);
    }
}