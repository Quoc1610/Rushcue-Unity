using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerManager : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Enter Trigger with: {other.gameObject.name}");
        if (other.CompareTag("Animal"))
        {
            Debug.Log("Animal detected");
            PlayerManager.Instance().scanner.SetActive(true);
        }
        else
        {
            Debug.Log("Tag is not 'Animal'");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exit Trigger with: {other.gameObject.name}");
        if (other.CompareTag("Animal"))
        {
            Debug.Log("Animal left trigger");
            PlayerManager.Instance().scanner.SetActive(false);
        }
        else
        {
            Debug.Log("Tag is not 'Animal'");
        }
    }
}
