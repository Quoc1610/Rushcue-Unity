using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UILoading : MonoBehaviour
{
    [SerializeField] private Slider sliderLoad;
    [SerializeField] private TextMeshProUGUI txtLoading;

    public void OnSetUp()
    {
        sliderLoad.value = 0;
        sliderLoad.maxValue = 3;
        txtLoading.text = "Loading...";

    }
    IEnumerator LoadScene()
    {
        txtLoading.text = "Done!";
        UIManager.Instance().uiMainMenu.gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        this.gameObject.SetActive(false);

    }
    private void Update()
    {
        sliderLoad.value += Time.deltaTime;
    }
    public void OnChangeValueSlider()
    {
        int percentage = Mathf.RoundToInt((sliderLoad.value / sliderLoad.maxValue) * 100);
        txtLoading.text = $"Loading... {percentage}%";
        if (sliderLoad.value == sliderLoad.maxValue)
        {
           StartCoroutine(LoadScene());
        }
    }
}
