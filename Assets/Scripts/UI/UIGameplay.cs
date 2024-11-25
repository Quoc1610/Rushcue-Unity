using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplay : MonoBehaviour
{
    public FloatingJoystick joystick;

    public void OnSetUp()
    {
        joystick.OnPointerUp(null);
        joystick.Start();
        joystick.gameObject.SetActive(true);
    }
}
