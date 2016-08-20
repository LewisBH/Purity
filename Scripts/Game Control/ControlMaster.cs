using UnityEngine;
using System.Collections;

public class ControlMaster : MonoBehaviour {

    public static ControlMaster Instance;
    private float deadZone = 0.15f;

    public enum ControllerType
    {
        Keyboard,
        Controller
    };

    public ControllerType controllerType = ControllerType.Keyboard;

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

    void Start()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        #region CONTROLLER INPUTS
        Vector2 controllerInput = new Vector2(Input.GetAxisRaw("Controller Left Stick Horizontal"), Input.GetAxisRaw("Controller Left Stick Vertical"));
        float controllerJump = Input.GetAxisRaw("Controller Jump");
        float controllerWeaponSwitch = Input.GetAxisRaw("Controller Weapon Switch");
        float controllerAttack = Input.GetAxisRaw("Controller Attack");
        
        if(controllerType == ControllerType.Keyboard)
        {
            if(
                controllerInput.x > deadZone || controllerInput.x < -deadZone ||
                controllerInput.y > deadZone || controllerInput.y < -deadZone ||
                controllerJump == 1 ||
                controllerAttack == 1 ||
                controllerWeaponSwitch == 1)
            {
                //Debug.Log("Controller set to a Controller");
                controllerType = ControllerType.Controller;
            }
        }
        #endregion

        #region KEYBOARD INPUTS
        Vector2 keyboardInput = new Vector2(Input.GetAxisRaw("Keyboard Horizontal"), Input.GetAxisRaw("Keyboard Vertical"));
        float keyboardJump = Input.GetAxisRaw("Keyboard Jump");
        float keyboardWeaponSwitch = Input.GetAxisRaw("Keyboard Weapon Switch");
        float keyboardAttack = Input.GetAxisRaw("Keyboard Attack");

        if(keyboardJump == 1)
        {
            //Debug.Log("keyboard jump button pressed");
        }

        if(controllerJump == 1)
        {
            //Debug.Log("controller Jump button pressed");
        }

        if (controllerType == ControllerType.Controller)
        {
            if (
                keyboardInput.x > deadZone || keyboardInput.x < -deadZone ||
                keyboardInput.y > deadZone || keyboardInput.y < -deadZone ||
                keyboardJump == 1 ||
                keyboardAttack == 1 ||
                keyboardWeaponSwitch == 1)
            {
                //Debug.Log("the input was set to the Keyboard");
                controllerType = ControllerType.Keyboard;
            }
        }
        #endregion
    }
}
