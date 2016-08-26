using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageController : MonoBehaviour {

    public static ImageController Instance;

    [SerializeField]
    Image leftCharacterSlot;
    [SerializeField]
    Image rightCharacterSlot;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }

        leftCharacterSlot.enabled = false;
        rightCharacterSlot.enabled = false;
    }

    public void ChangeImage(int slot, Sprite image)
    {
        if(slot == 1)
        {
            leftCharacterSlot.sprite = image;
        }
        else
        {
            rightCharacterSlot.sprite = image;
        }
    }

    public void ShowImages(Sprite leftImage, Sprite rightImage)
    {
        leftCharacterSlot.enabled = true;
        rightCharacterSlot.enabled = true;

        leftCharacterSlot.sprite = leftImage;
        rightCharacterSlot.sprite = rightImage;
    }

    public void UnShowImages()
    {
        leftCharacterSlot.sprite = null;
        rightCharacterSlot.sprite = null;

        leftCharacterSlot.enabled = false;
        rightCharacterSlot.enabled = false;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
