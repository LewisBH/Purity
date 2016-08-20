using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatTextManager : MonoBehaviour {

    public GameObject textPrefab;
    public RectTransform canvasTransform;
    public float speed;
    public Vector3 direction;
    public float fadeTime;
    public bool crit;

    private static CombatTextManager instance;

    public static CombatTextManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CombatTextManager>(); 
            }
            
            return instance;
        }
    }

    public void CreateText(Vector3 Position, string text, Color colour)
    {
        GameObject sct = Instantiate(textPrefab, Position, Quaternion.identity) as GameObject;
        sct.transform.SetParent(canvasTransform);
        sct.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        sct.GetComponent<CombatText>().Initilise(speed, direction,fadeTime,crit);
        sct.GetComponent<Text>().text = text;
        sct.GetComponent<Text>().color = colour;
    }
}
