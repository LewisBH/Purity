using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using UnityEngine.SceneManagement;

public class SaveLists : MonoBehaviour {

    public static SaveLists Instance;

    private bool[] listFoundArray = new bool[3];

    //the below lists are used to have multiple lists inside them so i can control what lists i use for each scene/ level
    public List<SavedItemList> savedObjectList = new List<SavedItemList>();
    public SavedUpgrades savedUpgrades;
    public List<SavedEnemyList> savedEnemyList = new List<SavedEnemyList>();
    public List<SavedObsticalList> savedObsticalList = new List<SavedObsticalList>();

    public delegate void SaveDelegate(object sender, EventArgs args);
    public static event SaveDelegate SaveEvent;

    public PlayerStatistics savedPlayerData = new PlayerStatistics();

    public PlayerStatistics localCopyOfData;

    void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
	 
	}

    public void InitialiseSceneList()
    {
        CheckLists();

        int SceneID = SceneManager.GetActiveScene().buildIndex;

        //we need to find if we already have a list of saved items for this level.
        for (int i = 0; i < savedObjectList.Count; i++)
        {
            if (savedObjectList[i].SceneID == SceneID)
            {
                //print(savedObjectList[i].SavedSkillPoints.Count);
                listFoundArray[0] = true;
                print("Object list found for scene!");
            }
        }

        for (int i = 0; i < savedEnemyList.Count; i++)
        {
            if (savedObjectList[i].SceneID == SceneID)
            {
                //print(savedObjectList[i].SavedSkillPoints.Count);
                listFoundArray[1] = true;
                print("Enemy list for scene found");
            }
        }

        for (int i = 0; i < savedObsticalList.Count; i++)
        {
            if(savedObsticalList[i].SceneID == SceneID)
            {
                listFoundArray[2] = true;
                print("Scene for obsticals was found");
            }
        }

        for(int i = 0; i < listFoundArray.Length; i++)
        {
            if(i == 0)
            {
                if(!listFoundArray[i])
                {
                    SavedItemList newItemList = new SavedItemList(SceneID);
                    savedObjectList.Add(newItemList);
                }
            }
            else if (i == 1)
            {
                if (!listFoundArray[i])
                {
                    SavedEnemyList newEnemyList = new SavedEnemyList(SceneID);
                    savedEnemyList.Add(newEnemyList);
                }
            }
            else if (i == 2)
            {
                if (!listFoundArray[i])
                {
                    print("obstical list not found. creating a new one");
                    SavedObsticalList newObsticalList = new SavedObsticalList(SceneID);
                    savedObsticalList.Add(newObsticalList);
                }
            }
        }
    }

    void CheckLists()
    {
        if (savedObjectList == null)
        {
            print("Saved Object list was null");
            savedObjectList = new List<SavedItemList>();
        }

        if (savedEnemyList == null)
        {
            print("Saved Enemy List Was null");
            savedEnemyList = new List<SavedEnemyList>();
        }

        if (savedObsticalList == null)
        {
            print("Saved Obstical List Was null");
            savedObsticalList = new List<SavedObsticalList>();
        }
    }

    public SavedItemList GetItemListForScene()
    {
        for (int i = 0; i < savedObjectList.Count; i++)
        {
            if (savedObjectList[i].SceneID == SceneManager.GetActiveScene().buildIndex)
            {
                //print("got the scene list");
                return savedObjectList[i];

            }
        }
        return null;
    }

    public void ClearObsticalList()
    {
        for (int i = 0; i < savedObsticalList.Count; i++)
        {
            savedObsticalList[i].savedObsticals.Clear();
        }
    }

    public SavedObsticalList GetObsticalListForScene()
    {
        for (int i = 0; i < savedObsticalList.Count; i++)
        {
            if(savedObsticalList[i].SceneID == SceneManager.GetActiveScene().buildIndex)
            {
                return savedObsticalList[i];
            }
        }
        return null;
    }

   

    public SavedEnemyList GetEnemyListForScene()
    {
        for (int i = 0; i < savedEnemyList.Count; i++)
        {
            if (savedEnemyList[i].SceneID == SceneManager.GetActiveScene().buildIndex)
            {
               // print("got the enemy list");
                return savedEnemyList[i];
            }
        }
        return null;
    }

    public SavedUpgrades GetUpgradeList()
    {
        return savedUpgrades;
    }

    public void FireSaveEvent()
    {
        GetItemListForScene().SavedSkillPoints = new List<SavedPickUpableSkillPoint>();
        GetEnemyListForScene().savedBosses = new List<SavedBoss>();
        //If we have any functions in the event:
        if (SaveEvent != null)
        {
            SaveEvent(null, null);
        }

    }

    public void SaveData()
    {
        if (!Directory.Exists("Saves"))
        {
            Directory.CreateDirectory("Saves");
        }

        FireSaveEvent();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Create("Saves/saves.binary");
        FileStream saveObjects = File.Create("Saves/saveObjects.binary");

        localCopyOfData = PlayerState.Instance.localPlayerData;

        bf.Serialize(saveFile, localCopyOfData);
        bf.Serialize(saveObjects, savedObjectList);

        saveFile.Close();
        saveObjects.Close();

        //print("saved");
    }

    public void LoadData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/save.binary", FileMode.Open);
        FileStream saveObjects = File.Open("Saves/saveObjects.binary", FileMode.Open);

        localCopyOfData = (PlayerStatistics)bf.Deserialize(saveFile);
        savedObjectList = (List<SavedItemList>)bf.Deserialize(saveObjects);

        saveFile.Close();
        saveObjects.Close();

        //print("Loaded");
    }

    // Update is called once per frame
    void Update () {
	
	}
}
