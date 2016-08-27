using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {

    public static DialogueController Instance;
    public GameObject blurScreen;

    public Text textBox;

    private PlayerInput playerInput;

    public bool curInDialogue;

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
                print(dialogue[dialogueIndex].speach);

                //print("Dialogue index = " + dialogueIndex);
                //print("lines of dialogue left = " + linesOfDialogueLeft);

                textBox.text = dialogue[dialogueIndex].speach;

                print(dialogue[dialogueIndex].image.slotIndex);
                print(dialogue[dialogueIndex].image.imageIndex);

                ImageController.Instance.ChangeImage(dialogue[dialogueIndex].image.slotIndex,
                    characterSpriteList.spriteList[dialogue[dialogueIndex].image.imageIndex]);

                dialogueIndex++;
                linesOfDialogueLeft--;
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

            //print(dialogue[0].speach);
            //linesOfDialogueLeft--;
        }
    }

    void EndDialogue()
    {
        textBox.text = "";
        curInDialogue = false;
        playerInput.paused = false;
        blurScreen.SetActive(false);
        dialogueIndex = 0;
        ImageController.Instance.UnShowImages();
    }
}
