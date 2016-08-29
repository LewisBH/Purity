using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour {

    public static Player Instance;

    private WeaponSwitch weaponSwitchScript;

    public PlayerStatistics playerStatistics;

    public Stat HP;
    public Stat SP;

    public float maxJumpHeight = 4;
    public float minJumpHeight = 1;
    public float timeToJumpApex = 0.4f;

    public Transform weaponSlot;

    public GameObject dagger;
    public GameObject hammer;
    public GameObject sword;
    public GameObject rodOfLight;

    float accelerationTimeAirborne = 0.2f;
    float accelerationTimerGrounded = 0.1f;
    float moveSpeed = 6;
    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity = 0;
    float velocityXSmoothing;

    public bool jumping = false;
    public bool openedWeaponUI = false;

    Vector3 velocity;

    PlayerController controller;

    Vector2 directionalInput;
    
    void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;

            HP.Initilise();
            SP.Initilise();
        }
        else
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        weaponSwitchScript = GetComponentInChildren<WeaponSwitch>();
        controller = GetComponent<PlayerController>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpHeight = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpHeight = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        dagger.transform.parent = transform;
        dagger.transform.position = weaponSlot.transform.position;
	}

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimerGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
	
    public void OnJumpInputDown(float jumpVelocity)
    {
        velocity.y = maxJumpVelocity;
        jumping = true;
    }

    public void OnJumpInputUp(float jumpVelocity)
    {
        if (jumpVelocity == 0 && jumping)
        {
            if (velocity.y > minJumpVelocity)
            {
                velocity.y = minJumpVelocity;
                jumping = false;
            }
        }
    }

    public void OnWeaponUIDown()
    {
        weaponSwitchScript.OpenUI();
        openedWeaponUI = true;
    }

    public void OnWeaponUIUp()
    {
        openedWeaponUI = false;
        weaponSwitchScript.CloseUI();
    }

	// Update is called once per frame
	void Update () {

        CalculateVelocity();

        controller.Move(velocity * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if(Input.GetButtonUp("Save"))
        {
            Scene scene = SceneManager.GetActiveScene();
            PlayerState.Instance.localPlayerData.SceneID = scene.buildIndex;
            PlayerState.Instance.localPlayerData.PosX = transform.position.x;
            PlayerState.Instance.localPlayerData.PosY = transform.position.y;

            SaveLists.Instance.SaveData();
        }

        if(Input.GetButtonUp("Load"))
        {
            SaveLists.Instance.LoadData();
            GameControl.Instance.isSceneBeingLoaded = true;

            int whichScene = SaveLists.Instance.localCopyOfData.SceneID;

            SceneManager.LoadScene(whichScene);
        }
    }
}
