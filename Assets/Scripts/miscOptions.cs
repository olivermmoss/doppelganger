using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class miscOptions : MonoBehaviour
{
    public TextMeshProUGUI scrollText;
    public Slider scrollSpeedSlider;
    public Toggle fpsToggle;
    private Coroutine typing;
    private float yknowthetime;
    private TextMeshProUGUI fpsText;

    private void Start()
    {
        if(!PlayerPrefs.HasKey("scrollTime"))
        {
            PlayerPrefs.SetFloat("scrollTime", 0.025f);
        }
        if (!PlayerPrefs.HasKey("showFPS"))
        {
            PlayerPrefs.SetInt("showFPS", 0);
        }

        fpsText = GameObject.FindGameObjectWithTag("fps").GetComponent<TextMeshProUGUI>();

        bool FPYes = PlayerPrefs.GetInt("showFPS") == 1;
        fpsText.enabled = FPYes;
        fpsToggle.isOn = FPYes;
        yknowthetime = PlayerPrefs.GetFloat("scrollTime");
        scrollSpeedSlider.value = 1 - Mathf.InverseLerp(0.002f, 0.2f, yknowthetime);
    }

    public void SetTextScroll()
    {
        yknowthetime = Mathf.Lerp(0.002f, 0.2f, 1 - scrollSpeedSlider.value);
        PlayerPrefs.SetFloat("scrollTime", yknowthetime);

        if(typing != null)
            StopCoroutine(typing);

        typing = StartCoroutine(TypeSentence("Scroll Speed"));
    }

    IEnumerator TypeSentence(string sentence)
    {
        scrollText.text = "";


        foreach (char letter in sentence.ToCharArray())
        {
            scrollText.text += letter;
            yield return new WaitForSecondsRealtime(yknowthetime);
        }
    }

    public void ShowFPS(bool useless)
    {
        if(fpsText == null)
        {
            return;
        }
        bool FPYes = fpsToggle.isOn;
        PlayerPrefs.SetInt("showFPS", FPYes ? 1 : 0);
        fpsText.enabled = FPYes;
    }

    public void Die()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<playerHealth>().Die();
    }

    public void DeleteSave()
    {
        GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().ResetData();
    }
}
