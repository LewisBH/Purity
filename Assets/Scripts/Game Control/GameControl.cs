﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    public static GameControl Instance;
    public static GameObject StartMenu;
    public static GameObject SelectMenu;
   
    public Scene activeScene;

    public bool isSceneBeingLoaded = false;
    public bool isSceneBeingTransitioned = false;

    public Transform transitionTarget;

    [SerializeField]
    GameObject pauseMenu;

    [SerializeField]
    GameObject selectMenu;

    [SerializeField]
    GameObject selectDefaultButton;

    [SerializeField]
    GameObject startDefaultButton;

    bool isPaused;

    [SerializeField]
    Transform upgradeParent;

    public Upgrade[] upgradeScripts;

    void Awake()
    {
        if (Instance == null)//checks to see if there is a game control object and if there isnt one, make this that game object
        {
            DontDestroyOnLoad(gameObject);//stop this game object from being deleted between scenes
            Instance = this;//makes this game object the game control object in use
            activeScene = SceneManager.GetActiveScene();
            //Debug.Log(activeScene.name);
        }
        else if (Instance != this)// checks to see if there is a game object, and if there is one but it is not this game object 
        {
            Destroy(gameObject);// then it destroys itself
        }
    }

    void Start()
    {
        pauseMenu = CameraFollow.Instance.GetComponentInChildren<PauseMenu>(true).gameObject;
        selectMenu = CameraFollow.Instance.GetComponentInChildren<SelectMenu>(true).gameObject;
        startDefaultButton = CameraFollow.Instance.GetComponentInChildren<DefaultStartButton>(true).gameObject;
        selectDefaultButton = CameraFollow.Instance.GetComponentInChildren<DefaultSelectButton>(true).gameObject;

        upgradeScripts = upgradeParent.GetComponentsInChildren<Upgrade>();

        for (int i = 0; i < upgradeScripts.Length; i++) // this loops through all the upgrades and sets there id which increments by 1 every time it loops.
        {
            upgradeScripts[i].id = i;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        #region PAUSE
        bool start = Input.GetKeyDown(KeyCode.Joystick1Button7);
        bool select = Input.GetKeyDown(KeyCode.Joystick1Button6);

        if(start)
        {
            if(isPaused)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                selectMenu.SetActive(false);
                isPaused = false;
                EventSystem.current.SetSelectedGameObject(startDefaultButton);
            }
            else if(!isPaused)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                isPaused = true;
            }
        }

        if(select)
        {
            if(isPaused)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                selectMenu.SetActive(false);
                isPaused = false;
                EventSystem.current.SetSelectedGameObject(selectDefaultButton);
            }
            else if(!isPaused)
            {
                Time.timeScale = 0;
                selectMenu.SetActive(true);
                isPaused = true;
            }
        }
        #endregion PAUSE
    }

    public void OnLoad()
    {
        
    }
}