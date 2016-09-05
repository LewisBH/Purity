using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuInputManager : MonoBehaviour {

    public static MenuInputManager Instance;

    public Button selectedButton;

    private string whichAxisX;
    private string whichAxisY;
    private string whichJump;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (ControlMaster.Instance.controllerType == ControlMaster.ControllerType.Keyboard)
        {
            whichAxisX = "Keyboard Horizontal";
            whichAxisY = "Keyboard Vertical";
            whichJump = "Keyboard Jump";
        }
        else
        {
            whichAxisX = "Controller Left Stick Horizontal";
            whichAxisY = "Controller Left Stick Vertical";
            whichJump = "Controller Jump";
        }

    }
}
