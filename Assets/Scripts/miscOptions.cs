using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class miscOptions : MonoBehaviour
{
    public TextMeshProUGUI scrollText;
    private dontDestroySave save;
    public Slider scrollSpeedSlider;
    private Coroutine typing;

    private void Start()
    {
        save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
        scrollSpeedSlider.value = 1 - Mathf.InverseLerp(0.01f, 0.1f, save.timeToTextScroll);
    }

    public void SetTextScroll()
    {
        save.timeToTextScroll = Mathf.Lerp(0.002f, 0.2f, 1 - scrollSpeedSlider.value);

        print(save.timeToTextScroll);

        if(typing != null)
            StopCoroutine(typing);

        typing = StartCoroutine(TypeSentence("Scroll Speed"));
    }

    IEnumerator TypeSentence(string sentence)
    {
        scrollText.text = "";


        foreach (char letter in sentence.ToCharArray())
        {
            print("added letter" + letter);
            scrollText.text += letter;
            yield return new WaitForSecondsRealtime(save.timeToTextScroll);
        }
    }
}
