using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class dontDestroySave : MonoBehaviour
{
    public string stasisScene;
    public Vector2 stasisCoords;
    public Vector2 stasisMaxes;
    public Vector2 stasisMins;

    public bool gustavKilled;
    public bool[] itemsGotten = new bool[10] { false, false, false, false, false, false, false, false, false, false };
    public bool[] bossesKilled = new bool[10] { false, false, false, false, false, false, false, false, false, false };
    public bool[] cutscenesWatched = new bool[10] { false, false, false, false, false, false, false, false, false, false };
    public bool[] skullGate = new bool[10] { false, false, false, false, false, false, false, false, false, false };
    public bool[] maxHealthIncreases = new bool[10] { false, false, false, false, false, false, false, false, false, false };

    public Vector2 NewScenePos;
    public bool facingRight;
    public Vector2 camMins;
    public Vector2 camMaxes;

    public int playerHealth = -20;
    public float playerFireCharge = 0;
    public bool bossQuick = false;
    public int playerMaxHealth = 3;

    public GameObject player;
    public bool playerDead;

    //debug variable
    public bool quickStart = false;
    public bool useSaveFile;

    AsyncOperation asyncLoad;
    bool loadingScene = false;
    public float wipeTIme = 1;
    float wipeTimer;
    public bool verticalWipe;
    public bool wipePosDiff;

    bool DoThings()
    {
        loadingScene = false;

        if (quickStart)
        {
            gustavKilled = true;
            itemsGotten[0] = true;
            itemsGotten[1] = true;
            itemsGotten[2] = true;
            cutscenesWatched[0] = true;
            cutscenesWatched[1] = true;
        }

        Time.timeScale = 1;
        CameraSystem cam = GameObject.Find("CameraMover").GetComponent<CameraSystem>();
        DontDestroyOnLoad(gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
        string sceneName = SceneManager.GetActiveScene().name;

        string rebinds = PlayerPrefs.GetString("rebinds");
        if(!string.IsNullOrEmpty(rebinds))
            player.GetComponent<PlayerMove>().actions.LoadBindingOverridesFromJson(rebinds);

        if(sceneName == "MainMenu")
        {
            return true;
        }

        if (itemsGotten[0])
        {
            player.GetComponent<playerAttack>().enabled = true;
        }
        else
        {
            player.GetComponent<playerAttack>().enabled = false;
        }
        if (itemsGotten[1] && sceneName != "Mountain1")
        {
            player.GetComponent<playerFire>().enabled = true;
            GameObject.FindGameObjectWithTag("fireball").transform.GetChild(0).gameObject.SetActive(true);
        }
        

        if(sceneName == "DarkCastle")
        {
            if(gustavKilled)
            {
                GameObject.Find("goodGustavOverworld").GetComponent<goodGustavKill>().DoIt();
            }
            else
            {
                GameObject.Find("stasisTank").GetComponent<statisTankController>().Activate();
                //StartCoroutine(GameObject.Find("stasisTank").GetComponent<statisTankController>().Respawn());
                //playerDead = true;
            }
        }
        if(sceneName == "Wasteland1")
        {
            if(itemsGotten[0])
            {
                GameObject.Find("swordShrine").GetComponent<swordShrine>().DoIt(false);
            }
            if (!bossesKilled[0])
            {
                GameObject.Find("wastelandBridge").SetActive(false);
                GameObject.Find("builderOW").GetComponent<SpriteRenderer>().flipX = true;
            }

        }
        if (sceneName == "WastelandCaves")
        {
            if (!bossesKilled[0])
            {
                GameObject.Find("wastelandLadder_2").SetActive(false);
            }
        }
        if (sceneName == "Mountain1")
        {
            if (itemsGotten[1])
            {
                FindObjectOfType<swordShrine>().DoIt(false);
            }
        }
        if (sceneName == "Mountain5")
        {
            if (itemsGotten[2])
            {
                FindObjectOfType<swordShrine>().DoIt(false);
            }
        }
        //death behavior
        if (playerDead && stasisScene != null && (stasisScene != "" && stasisScene == sceneName) || (stasisScene == "" && sceneName == "DarkCastle"))
        {
            //print(stasisCoords);
            //put player in right spot
            player.transform.position = stasisCoords;
            cam.xMin = stasisMins.x;
            cam.xMax = stasisMaxes.x;
            cam.yMin = stasisMins.y;
            cam.yMax = stasisMaxes.y;

            //active existing stasis chambers
            Collider2D hitCollider = Physics2D.OverlapCircle(stasisCoords, 0.5f);
            StartCoroutine(hitCollider.GetComponent<statisTankController>().Respawn());
        }
        //scene change behavior
        else if(NewScenePos != null && gustavKilled && !quickStart)
        {
            //active existing stasis chambers
            if(stasisScene == sceneName)
            {
                Collider2D hitCollider = Physics2D.OverlapCircle(stasisCoords, 0.5f);
                hitCollider.GetComponent<statisTankController>().Activate();
            }
            player.transform.position = NewScenePos;
            cam.xMin = camMins.x;
            cam.xMax = camMaxes.x;
            cam.yMin = camMins.y;
            cam.yMax = camMaxes.y;
            if(facingRight)
            {
                player.transform.localScale = new Vector2(1, 1);
            }
            else
            {
                player.transform.localScale = new Vector2(-1, 1);
            }
            GameObject.FindGameObjectWithTag("screenWipe").GetComponent<screenWipeController>().WipeOff(verticalWipe, wipePosDiff);
        }
        //default
        else
        {
            //player spawns where avatar is
            quickStart = false;
        }

        return false;
    }

    void ChangedActiveScene(Scene current, Scene next)
    {
        if (current != next)
        {
            if(DoThings())
            {
                return;
            }

            player.GetComponent<playerHealth>().maxHealth = playerMaxHealth;
            if (playerHealth != -20 && !playerDead)
            {
                
                player.GetComponent<playerHealth>().health = playerHealth;
                //player.GetComponent<playerHealth>().UpdateHearts();
                //print("setHealth");

                if (itemsGotten[1])
                {
                    player.GetComponent<playerFire>().charge = playerFireCharge;
                }
            }
            else
            {
                player.GetComponent<playerHealth>().health = playerMaxHealth;
            }

            playerDead = false;
        }
    }
    void Awake()
    {
        playerDead = true;

        SceneManager.activeSceneChanged += ChangedActiveScene;

        if(useSaveFile)
            LoadGame();

        //DoThings();
    }

    public void ChangeScene(string doorExit, Vector2 _NewScenePos, bool _facingRight, Vector2 _camMins, Vector2 _camMaxes)
    {
        if (loadingScene)
            return;
        loadingScene = true;

        if (player != null)
        {
            playerHealth = player.GetComponent<playerHealth>().health;
            if (itemsGotten[1])
            {
                playerFireCharge = player.GetComponent<playerFire>().charge;
            }
        }

        NewScenePos = _NewScenePos;
        facingRight = _facingRight;
        camMins = _camMins;
        camMaxes = _camMaxes;
        StartCoroutine(LoadAsyncScene(doorExit));
    }

    IEnumerator LoadAsyncScene(string doorExit)
    {
        print(doorExit);
        wipeTimer = Time.time;
        asyncLoad = SceneManager.LoadSceneAsync(doorExit, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;
        //wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            //scene has loaded as much as possible,
            // the last 10% can't be multi-threaded
            if (asyncLoad.progress >= 0.9f && Time.time > wipeTimer + wipeTIme)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    //https://www.red-gate.com/simple-talk/development/dotnet-development/saving-game-data-with-unity/
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath
                     + "/savedata.dg");
        SaveData data = new SaveData();

        data.stasisScene = stasisScene;
        data.stasisCoords = new float[2] {stasisCoords.x, stasisCoords.y};
        data.stasisMaxes = new float[2] { stasisMaxes.x, stasisMaxes.y }; ;
        data.stasisMins = new float[2] { stasisMins.x, stasisMins.y }; ;
        data.gustavKilled = gustavKilled;
        data.itemsGotten = itemsGotten;
        data.bossesKilled = bossesKilled;
        data.cutscenesWatched = cutscenesWatched;
        data.skullGate = skullGate;
        data.maxHealthIncreases = maxHealthIncreases;
        data.playerMaxHealth = playerMaxHealth;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");

        GameObject saveIcon = GameObject.FindGameObjectWithTag("saveIcon");
        if (saveIcon == null)
            return;
        saveIcon.GetComponent<Animator>().SetTrigger("save");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath
                       + "/savedata.dg"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
                       File.Open(Application.persistentDataPath
                       + "/savedata.dg", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();

            stasisScene = data.stasisScene;
            stasisCoords = new Vector2(data.stasisCoords[0], data.stasisCoords[1]);
            stasisMaxes = new Vector2(data.stasisMaxes[0], data.stasisMaxes[1]);
            stasisMins = new Vector2(data.stasisMins[0], data.stasisMins[1]);
            gustavKilled = data.gustavKilled;
            itemsGotten = data.itemsGotten;
            bossesKilled = data.bossesKilled;
            cutscenesWatched = data.cutscenesWatched;
            skullGate = data.skullGate;
            maxHealthIncreases = data.maxHealthIncreases;
            playerMaxHealth = data.playerMaxHealth;

            Debug.Log("Game data loaded!");
        }
        else
        {
            Debug.LogWarning("There is no save data!");
            stasisScene = "DarkCastle";
            stasisCoords = new Vector2(-72f, -2f);
            stasisMaxes = new Vector2(-55f, 0f);
            stasisMins = new Vector2(-66f, 0f);
            SaveGame();
        }
    }

        public void ResetData()
        {
            if (File.Exists(Application.persistentDataPath
                          + "/savedata.dg"))
            {
                File.Delete(Application.persistentDataPath
                                  + "/savedata.dg");
            
                Debug.Log("Data reset complete!");
            }
            else
                Debug.LogError("No save data to delete.");
        }
}

[Serializable]
class SaveData
{
    public string stasisScene;
    //vector2s didn't work so it's float[]s
    public float[] stasisCoords;
    public float[] stasisMaxes;
    public float[] stasisMins;

    public bool gustavKilled;
    public bool[] itemsGotten;
    public bool[] bossesKilled;
    public bool[] cutscenesWatched;
    public bool[] skullGate;
    public bool[] maxHealthIncreases;

    public int playerMaxHealth = 3;
}
