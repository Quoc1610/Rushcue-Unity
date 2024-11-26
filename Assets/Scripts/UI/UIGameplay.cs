using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameplay : MonoBehaviour
{
    public FloatingJoystick joystick;
    public Button btnClearData;

    public void OnSetUp()
    {
        joystick.OnPointerUp(null);
        joystick.Start();
        joystick.gameObject.SetActive(true);
    }
    public void OnClearData_Clicked()
    {
        SaveManager.ClearData();
        SaveManager.SaveData(new SaveData());
    }
}
