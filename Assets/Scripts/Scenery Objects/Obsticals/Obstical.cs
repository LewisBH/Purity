using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class Obstical : MonoBehaviour{

    Animation animator;
    AnimationClip transitionAnim;
    SpriteRenderer spriteRenderer;

    public enum state
    {
        NotRemoved,
        Removed
    }

    public state curState;

	// Use this for initialization
	public virtual void Start () {
        SaveLists.SaveEvent += SaveFunction;
        spriteRenderer = GetComponent<SpriteRenderer>();

        ObjectLists.Instance.obsticalList.Add(gameObject);
        print("added to obstical list");

        if(curState == state.Removed)
        {
            spriteRenderer.enabled = false;
        }
	}
	public void OnDestroy()
    {
        SaveLists.SaveEvent -= SaveFunction;
    }

    public void SaveFunction(object sender, EventArgs args)
    {
        SaveLists.Instance.ClearObsticalList();

        SavedObstical obstical = new SavedObstical();

        obstical.curState = curState;
        obstical.name = gameObject.name;

        print(SaveLists.Instance.GetObsticalListForScene().savedObsticals.Count);
        SaveLists.Instance.GetObsticalListForScene().savedObsticals.Add(obstical);
        print(SaveLists.Instance.GetObsticalListForScene().savedObsticals.Count);
        //print(name + " has saved");
    }

    public void TransitionState()
    {
        curState = state.Removed;
    }
}
