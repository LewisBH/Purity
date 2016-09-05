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

    private int faceDir;
    private string direction = "right";

    private Transform graphics;

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

        Debug.LogError("The player animations aren't playing, but the attacking still works. look at the old animations when you get the new animations");

        graphics = GetComponentInChildren<MeshRenderer>().transform;
        weaponSwitchScript = GetComponentInChildren<WeaponSwitch>();
        controller = GetComponent<PlayerController>();

        gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpHeight = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpHeight = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

        dagger.transform.parent = graphics.transform;
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

        faceDir = (int)Mathf.Sign(velocity.x);

        Vector3 newDirection = graphics.localScale;
        newDirection.x = faceDir;
        graphics.localScale = newDirection;
        weaponSlot.localScale = newDirection;

        if(faceDir == -1 && direction == "right")
        {
            Vector3 tempScale = weaponSlot.localScale;
            tempScale.x *= faceDir;
            weaponSlot.localScale = tempScale;
            direction = "left";
        }
        else if (faceDir == 1 && direction == "left")
        {
            Vector3 tempScale = weaponSlot.localScale;
            tempScale.x *= faceDir;
            weaponSlot.localScale = tempScale;
            direction = "right";
        }

        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if(velocity.x > 0)
        {

        }
    }
}
