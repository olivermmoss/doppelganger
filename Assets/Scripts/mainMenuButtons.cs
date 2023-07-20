using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.InputSystem;

public class mainMenuButtons : MonoBehaviour
{
    private dontDestroySave save;
    //public InputActionAsset actions;

    // Start is called before the first frame update
    void Start()
    {
        save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
    }

    public void ContinueButton()
    {
        //save.LoadGame();
        //print(save.stasisCoords);
        save.ChangeScene(save.stasisScene, save.stasisCoords, true, save.stasisMins, save.stasisMaxes);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
