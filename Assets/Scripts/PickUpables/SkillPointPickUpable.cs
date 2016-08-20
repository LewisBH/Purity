using UnityEngine;
using System.Collections;
using System;

public class SkillPointPickUpable : MonoBehaviour {

    public bool hasBeenPickedUp = false;

	// Use this for initialization
	void Start () {
        SaveLists.SaveEvent += SaveFunction;
	}
	
	public void OnDestroy()
    {
        SaveLists.SaveEvent -= SaveFunction;
    }

    public void SaveFunction(object sender, EventArgs args)
    {
        SavedPickUpableSkillPoint skillPoint = new SavedPickUpableSkillPoint();
        skillPoint.PickedUp = hasBeenPickedUp;

        SaveLists.Instance.GetItemListForScene().SavedSkillPoints.Add(skillPoint);
    }
}
