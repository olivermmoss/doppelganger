using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class dialogueManager : MonoBehaviour
{
    private static dialogueManager instance;
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    private Story currentStory;
    private bool dialogueIsPlaying = false;
    public GameObject player;
    public GameObject[] healthIcons;
    private bool typing = true;
    private dialogueTrigger thisTrig;
    private talkingCutsceneTrigger thisTrig2;
    public dontDestroySave save;

    private const string PORTRAITTAG = "portrait";
    private const string LAYOUTTAG = "layout";
    private const string SOUNDTAG = "sfx";
    private const string SPRITECHANGETAG = "spritechange";
    private const string EVENTTAG = "event";
    public Animator portraitAnimator;
    public Animator layoutAnimator;
    public AudioSource source;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("found more than one dialogue manager");
        }
        instance = this;
        player = GameObject.FindGameObjectWithTag("Player");
        var healths = GameObject.FindGameObjectWithTag("healthUI").transform;
        healthIcons = new GameObject[healths.childCount + 1];
        for(int i = 0; i<healths.childCount; i++)
        {
            healthIcons[i] = healths.GetChild(i).gameObject;
        }
        healthIcons[healths.childCount] = GameObject.Find("objectToFind");
    }

    private void Start()
    {
        save = GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>();
    }

    public static dialogueManager GetInstance()
    {
        return instance;
    }

    public void EnterDialogueMode(TextAsset inkJson, dialogueTrigger trig = null, talkingCutsceneTrigger trig2 = null)
    {
        thisTrig = trig;
        thisTrig2 = trig2;
        currentStory = new Story(inkJson.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        foreach(GameObject healthIcon in healthIcons)
        {
            healthIcon.SetActive(false);
        }
        player.GetComponent<PlayerMove>().canMove = false;
        typing = false;

        ContinueStory();    
    }

    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        foreach(GameObject healthIcon in healthIcons)
        {
            healthIcon.SetActive(true);
        }
        player.GetComponent<PlayerMove>().canMove = true;
        print("exited");
        typing = false;
    }

    private void Update()
    {
        if(!dialogueIsPlaying)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            ContinueStory();
        }
    }

    void ContinueStory()
    {
        if(currentStory.canContinue && !typing)
        {
            StartCoroutine(TypeSentence(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
            typing = true;
        } else if (typing)
        {
            StopAllCoroutines();
            typing = false;
            dialogueText.text = currentStory.currentText;
        }
        else
        {
            ExitDialogueMode();
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach(string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("screw you lmao");
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKey)
            {
                case PORTRAITTAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUTTAG:
                    layoutAnimator.Play(tagValue);
                    source.panStereo = (tagValue == "left" ? -1 : 1);
                    break;
                case SOUNDTAG:
                    if (thisTrig != null)
                    {
                        source.pitch = Random.Range(0.7f, 1.3f);
                        source.clip = thisTrig.clips[int.Parse(tagValue)];
                    }
                    else if (thisTrig2 != null)
                    {
                        source.pitch = Random.Range(0.7f, 1.3f);
                        source.clip = thisTrig2.clips[int.Parse(tagValue)];
                    }
                    source.Play();
                    break;
                case SPRITECHANGETAG:
                    if(thisTrig != null)
                    {
                        thisTrig.spritesToChange[int.Parse(tagValue)].sprite = thisTrig.sprites[int.Parse(tagValue)];
                    }
                    else if (thisTrig2 != null)
                    {
                        thisTrig2.spritesToChange[int.Parse(tagValue)].sprite = thisTrig2.sprites[int.Parse(tagValue)];
                    }
                    break;
                case EVENTTAG:
                    //0 does a swordshrine
                    //1 changes something
                    if (thisTrig != null)
                    {
                        if (int.Parse(tagValue) == 0)
                        {
                            thisTrig.gameObject.GetComponent<swordShrine>().DoIt(true);
                        }
                        else if(int.Parse(tagValue) == 1)
                        {
                            thisTrig.gameObject.GetComponent<goodGustavKill>().DoIt();
                        }
                        else if (int.Parse(tagValue) == 2)
                        {
                            GameObject.FindGameObjectWithTag("save").GetComponent<dontDestroySave>().cutscenesWatched[1] = true;
                        }
                        else if (int.Parse(tagValue) == 3)
                        {
                            thisTrig.gameObject.GetComponent<doorController>().enabled = true;
                            thisTrig.gameObject.GetComponent<dialogueTrigger>().enabled = false;
                        }
                    }
                    break;
                default:
                    print("no tag");
                    break;
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(save.timeToTextScroll);
        }
        typing = false;
    }
}
