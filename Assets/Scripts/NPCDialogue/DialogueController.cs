using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DialogueController : MonoBehaviour {

    public static DialogueController Instance;
    public GameObject blurScreen;

    public bool curInDialogue;

    private PlayerInput playerInput;

    CharacterSpriteList characterSpriteList;
    float fadeSpeed;

    public List<Conversation> conversations;// this is all of the dialogue

    List<Dialogue> dialogue;// this is the current dialogue in use

    int dialogueIndex = 0;
    int linesOfDialogueLeft = 0;

    // Use this for initialization
    void Start () {

        //StartDialogue(0); this is how to call some dialogue

        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        playerInput = Player.Instance.gameObject.GetComponent<PlayerInput>();

        characterSpriteList = GetComponent<CharacterSpriteList>();
    }
	
	// Update is called once per frame
	void Update () {
	
        if(curInDialogue && playerInput.CheckAttack())
        {
            if(linesOfDialogueLeft != 0)
            {
                dialogueIndex++;
                print("dialogue index = " + dialogueIndex);
                linesOfDialogueLeft--;
                print("lines of dialogue left = " + linesOfDialogueLeft);
                print(dialogue[dialogueIndex].speach);
            }
            else
            {
                EndDialogue();
            }
        }

	}

    public void StartDialogue(int conversationID)
    {
        if(!curInDialogue)
        {
            dialogue = conversations[conversationID].dialogue;
            playerInput.paused = true;
            curInDialogue = true;
            blurScreen.SetActive(true);
            linesOfDialogueLeft = dialogue.Count;
            print(linesOfDialogueLeft);

            ImageController.Instance.ShowImages(
                characterSpriteList.spriteList[conversations[conversationID].initialImages.imageA],
                characterSpriteList.spriteList[conversations[conversationID].initialImages.imageB]);

            print(dialogue[0].speach);
            linesOfDialogueLeft--;
        }
    }

    /// <summary>
    /// TO DO: add to the xml document to allow character sprite change per each new line of dialogue.
    /// create somthing in the xml document to select which sprite slot needs to be changed.
    /// then on evey new line of dialogue check if the image has changed, if so, change it to the new one. 
    /// </summary>

    void EndDialogue()
    {
        curInDialogue = false;
        playerInput.paused = false;
        blurScreen.SetActive(false);
        ImageController.Instance.UnShowImages();
    }
}
