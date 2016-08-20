using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour {

    private Player player;
    private PlayerController controller;
    private Attack attackScript;

    public bool paused;

    private string whichAxisX = "";
    private string whichAxisY = "";
    private string whichJump = "";
    private string whichWeaponSwitch = "";
    private string whichAttack = "";
    private string whichSecondaryAttack = "";

    Vector2 directionalInput;
    float jump;
    float weaponSwitch;
    bool attack;
    bool secondaryAttack;

    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
        controller = GetComponent<PlayerController>();
        attackScript = GetComponent<Attack>();
    }
	
	// Update is called once per frame
	void Update () {

        if (ControlMaster.Instance.controllerType == ControlMaster.ControllerType.Keyboard)
        {
            whichAxisX = "Keyboard Horizontal";
            whichAxisY = "Keyboard Vertical";
            whichJump = "Keyboard Jump";
            whichWeaponSwitch = "Keyboard Weapon Switch";
            whichAttack = "Keyboard Attack";
            whichSecondaryAttack = "Keyboard Secondary Attack";
        }
        else
        {
            whichAxisX = "Controller Left Stick Horizontal";
            whichAxisY = "Controller Left Stick Vertical";
            whichJump = "Controller Jump";
            whichWeaponSwitch = "Controller Weapon Switch";
            whichAttack = "Controller Attack";
            whichSecondaryAttack = "Controller Secondary Attack";
        }

        directionalInput = new Vector2(Input.GetAxisRaw(whichAxisX), Input.GetAxisRaw(whichAxisY));
        jump = Input.GetAxisRaw(whichJump);
        weaponSwitch = Input.GetAxis(whichWeaponSwitch);
        attack = Input.GetButtonDown(whichAttack);
        secondaryAttack = Input.GetButtonDown(whichSecondaryAttack);

        if (!paused)
        {
            #region MOVEMENT
            if (directionalInput.x < 0.15f && directionalInput.x > -0.15f) //Dead Zone X Axis
            {
                directionalInput.x = 0;
            }
            if (directionalInput.y < 0.15f && directionalInput.y > -0.15)//Dead Zone Y Axis
            {
                directionalInput.y = 0;
            }

            player.SetDirectionalInput(directionalInput);

            #endregion

            #region JUMPING
            if (jump > 0.9f && controller.collisions.below)
            {
                player.OnJumpInputDown(jump);
                player.jumping = true;
            }

            if (jump == 0 && player.jumping)
            {
                player.OnJumpInputUp(jump);
            }

            #endregion

            #region WEAPON_SWITCH

            if (weaponSwitch == 1 && !player.openedWeaponUI)
            {
                player.OnWeaponUIDown();
            }
            else if (weaponSwitch == 0 && player.openedWeaponUI)
            {
                //Debug.Log("Controller trigger is equal to: " + weaponSwitch);
                player.OnWeaponUIUp();
            }
            #endregion

            #region ATTACK

            if (attack)
            {
                attackScript.RegularAttack();
            }

            if (secondaryAttack)
            {

            }

            #endregion
        }

    }

    public bool CheckAttack()
    {
        if(attack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
