using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class keyRebindButtons : MonoBehaviour
{
    public TextMeshProUGUI currentText;
    public string keybind;
    private InputActionAsset actions;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private bool rebinding = false;
    public int bindingIndex;
    public string actionMap = "gameplay";

    void Start()
    {
        actions = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>().actions;

        SetText();
    }

    public void SetText()
    {
        currentText.text = actions.FindActionMap(actionMap).FindAction(keybind).GetBindingDisplayString(bindingIndex);
    }

    public void RebindStart()
    {
        if(rebinding)
        {
            RebindComplete();
            return;
        }
        rebinding = true;

        currentText.text = "listening";

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("menu");

        rebindingOperation = actions.FindActionMap(actionMap).FindAction(keybind).PerformInteractiveRebinding(bindingIndex)
            .WithControlsExcluding("<Keyboard>/printScreen")
            .WithControlsExcluding("Mouse")
            .OnMatchWaitForAnother(0.1f)
            .OnComplete(operation => RebindComplete())
            .Start();
    }

    private void RebindComplete()
    {
        rebindingOperation.Dispose();

        string saveBinds = actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds", saveBinds);

        rebinding = false;

        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>().SwitchCurrentActionMap("gameplay");

        SetText();
    }

    public void ResetAll()
    {
        foreach (InputActionMap map in actions.actionMaps)
        {
            map.RemoveAllBindingOverrides();
        }
        foreach(keyRebindButtons key in FindObjectsOfType<keyRebindButtons>())
        {
            key.SetText();
        }
        PlayerPrefs.DeleteKey("rebinds");
    }
}
