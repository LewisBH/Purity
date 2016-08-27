using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class DialogueController : MonoBehaviour {

    public static DialogueController Instance;
    public GameObject blurScreen;

    [SerializeField]
    private GameObject textBox;
    [SerializeField]
    public Text text;

    private bool isTyping = false;
    private bool cancelTyping = false;

    [SerializeField]
    private float textScrollSpeed;

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
            if(!isTyping)
            {
                if(dialogueIndex > linesOfDialogueLeft)
                {
                    EndDialogue();
                }
                else
                {
                    ImageController.Instance.ChangeImage(dialogue[dialogueIndex].image.slotIndex,
                        characterSpriteList.spriteList[dialogue[dialogueIndex].image.imageIndex]);

                    StartCoroutine(TextScroll(dialogue[dialogueIndex].speach));
                }
                dialogueIndex++;
                linesOfDialogueLeft--;
            }

            else if(isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }

	}

    public void StartDialogue(int conversationID)
    {
        if(!curInDialogue)
        {
            textBox.SetActive(true);
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

    private IEnumerator TextScroll(string lineOfText)
    {
        int letter = 0;
        text.text = "";
        isTyping = true;
        cancelTyping = false;

        while(isTyping && !cancelTyping && (letter < lineOfText.Length - 1))
        {
            text.text += lineOfText[letter];
            letter++;
            yield return new WaitForSeconds(textScrollSpeed);
        }
        text.text = lineOfText;
        isTyping = false;
        cancelTyping = false;

    }

    void EndDialogue()
    {
        textBox.SetActive(false);
        text.text = "";
        curInDialogue = false;
        playerInput.paused = false;
        blurScreen.SetActive(false);
        dialogueIndex = 0;
        ImageController.Instance.UnShowImages();
    }
}
