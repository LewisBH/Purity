using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WeaponSwitch : MonoBehaviour {

    
    //Opening and Closing variables
    public bool fadingIn;

    public bool fadingOut;

    private SpriteRenderer[] spriteRenderers = new SpriteRenderer[4];

    private float minimum = 0.0f;
    private float maximum = 1f;
    private float duration = 0.3f;
    private float startTime;

    private float closedPos;
    private float openPos;
    private float curPos;

    //rest of the variables

    public bool isOpen = false;

    public Transform weaponSlot;
    public Transform player;

    public GameObject dagger;
    public GameObject sword;
    public GameObject rodOfLight;
    public GameObject hammer;

    public GameObject curWeapon;

    private Attack attackScript;

    private Vector3 resetPosition = new Vector3(-900, -900, -900);

	// Use this for initialization
	void Start () {
        curWeapon = dagger;

        hammer.transform.position = resetPosition;
        sword.transform.position = resetPosition;
        rodOfLight.transform.position = resetPosition;

        attackScript = player.GetComponent<Attack>();
        attackScript.ChangeWeaponTo(curWeapon.GetComponent<Weapon>());
        attackScript.Initialise();
        closedPos = transform.position.y;
        openPos = closedPos + 0.5f;

        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        for(int i = 0; i < 4; i++)
        {
            spriteRenderers[i].color = new Color(1f, 1f, 1f, 0f);
        }
	}
	
	// Update is called once per frame
	void Update ()
    { 
        if(fadingIn)
        {
            
            curPos = transform.position.y;
            float t = (Time.time - startTime) / duration;// sets t to the time then minus the time so it is basically 0
            float curAlpha = spriteRenderers[0].color.a;
            for (int i = 0; i < 4; i++)
            {
                spriteRenderers[i].color = new Color(1f, 1f, 1f, Mathf.SmoothStep(curAlpha, maximum, t));
            }

            transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(curPos, openPos, t), transform.position.z);

            if (spriteRenderers[3].color.a == 1)
            {
                fadingIn = false;
            }
        }

        if(fadingOut)
        {
            
            curPos = transform.position.y;
            float t = (Time.time - startTime) / duration;
            float curAlpha = spriteRenderers[0].color.a;
            for (int i = 0; i < 4; i++)
            {
                spriteRenderers[i].color = new Color(1f, 1f, 1f, Mathf.SmoothStep(curAlpha, minimum, t));

            }

            transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(curPos, closedPos, t), transform.position.z);

            if (spriteRenderers[3].color.a <= 0)
            {
                fadingOut = false;
            }
        }

        if(isOpen == true)
        {
            OnOpen();
        }

        curWeapon.transform.position = weaponSlot.transform.position;
        curWeapon.transform.rotation = weaponSlot.transform.rotation;
    }

    public void OpenUI()
    {
        isOpen = true;
        fadingIn = true;
        fadingOut = false;
        startTime = Time.time;
    }

    private void OnOpen()
    {
        string whichAxisX = "";
        string whichAxisY = "";

        if(ControlMaster.Instance.controllerType == ControlMaster.ControllerType.Keyboard)
        {
            whichAxisX = "Keyboard Horizontal";
            whichAxisY = "Keyboard Vertical";
        }
        else
        {
            whichAxisX = "Controller Right Stick Horizontal";
            whichAxisY = "Controller Right Stick Vertical";
        }

        Vector2 input = new Vector2(Input.GetAxisRaw(whichAxisX), Input.GetAxisRaw(whichAxisY));

        if (input.x > 0.9f)
        {
            curWeapon.transform.position = resetPosition;
            curWeapon.transform.parent = null;
            curWeapon = hammer;
            attackScript.ChangeWeaponTo(curWeapon.GetComponent<Weapon>());
            curWeapon.transform.parent = player;
            hammer.transform.position = weaponSlot.position;
            CloseUI();
        }
        else if (input.x < -0.9f)
        {
            curWeapon.transform.position = resetPosition;
            curWeapon.transform.parent = null;
            curWeapon = rodOfLight;
            attackScript.ChangeWeaponTo(curWeapon.GetComponent<Weapon>());
            curWeapon.transform.parent = player;
            rodOfLight.transform.position = weaponSlot.position;
            CloseUI();
        }
        else if (input.y > 0.9f)
        {
            curWeapon.transform.position = resetPosition;
            curWeapon.transform.parent = null;
            curWeapon = dagger;
            attackScript.ChangeWeaponTo(curWeapon.GetComponent<Weapon>());
            curWeapon.transform.parent = player;
            dagger.transform.position = weaponSlot.position;
            CloseUI();
        }
        else if (input.y < -0.9f)
        {
            curWeapon.transform.position = resetPosition;
            curWeapon.transform.parent = null;
            curWeapon = sword;
            attackScript.ChangeWeaponTo(curWeapon.GetComponent<Weapon>());
            curWeapon.transform.parent = player;
            sword.transform.position = weaponSlot.position;
            CloseUI();
        }
    }

    public void CloseUI()
    {
        isOpen = false;
        fadingOut = true;
        fadingIn = false;
        startTime = Time.time;
    }
}
