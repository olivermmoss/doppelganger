using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class volumeSliders : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer mixer;
    public string setting;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey(setting))
        {
            PlayerPrefs.SetFloat(setting, 0.8f);
            Load();
        }
        else { Load(); }
    }

    public void ChangeVolume()
    {
        mixer.SetFloat(setting, Mathf.Lerp(-80f, 5f, Mathf.Log10(volumeSlider.value * 10f/9f + 0.1f) + 1f));
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(setting);
        AudioListener.volume = volumeSlider.value;
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(setting, volumeSlider.value);
    }
}
