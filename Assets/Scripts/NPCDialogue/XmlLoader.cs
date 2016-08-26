using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class XmlLoader : MonoBehaviour {

    public TextAsset GameAsset;

    private DialogueController dialogueController;

    [SerializeField]
    private ImageController imageController;

    void Start()
    {
        dialogueController = GetComponent<DialogueController>();

        StartCoroutine(LoadDialogue(Path.Combine(Application.dataPath + "/Scripts/NPCDialogue/Dialogue/", "NPCTest1.xml")));

       //Debug.Log(conversations[1].id);
        //Debug.Log(conversations[1].dialogue[0].id);
        //Debug.Log(conversations[1].dialogue[0].name);
        //Debug.Log(conversations[1].dialogue[1].speach);
        //Debug.Log(conversations[1].dialogue[0].image.imageIndex);
        //Debug.Log(conversations[1].dialogue[0].image);

        //imageController.leftCharacterSlot.sprite = characterSpriteList[conversations[0].dialogue[0].image.imageIndex];
        //imageController.rightCharacterSlot.sprite = characterSpriteList[conversations[1].dialogue[1].image.imageIndex];

    }

    IEnumerator LoadDialogue(string category)
    {
        DialogueContainer dc = DialogueContainer.Load(category);

        if(dialogueController.conversations.Count > 0)
        {
            dialogueController.conversations.Clear();
        }

        foreach(Conversation item in dc.conversations)
        {
            dialogueController.conversations.Add(item);
        }

        yield return new WaitForSeconds(1);
    }
}