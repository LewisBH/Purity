using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Upgrade : MonoBehaviour
{
    [SerializeField]
    Player player;

    public Upgrade[] upgradeScript;
    public bool unlocked;
    public bool avaliable;
    public int id;

    [SerializeField]
    int prerequisitAmount;

    Color unlockedNormalColour = Color.white;
    Color avaliableNormalColour = Color.gray;
    Color unavaliableNormalColour = Color.black;

    

    public void Awake()
    {
        unlockedNormalColour = Color.white;
        avaliableNormalColour = Color.gray;
        unavaliableNormalColour = Color.black;

        UpdateColour();
    }

    void Start()
    {
        SaveLists.SaveEvent += SaveFunction;
    }

    void OnDestroy()
    {
        SaveLists.SaveEvent -= SaveFunction;
    }

    public void UpdateColour()
    {
        if (unlocked)
        {
            ColorBlock colour = GetComponent<Button>().colors;
            colour.normalColor = unlockedNormalColour;
            GetComponent<Button>().colors = colour;
        }
        else if (avaliable)
        {
            ColorBlock colour = GetComponent<Button>().colors;
            colour.normalColor = avaliableNormalColour;
            GetComponent<Button>().colors = colour;
        }
        else
        {
            ColorBlock colour = GetComponent<Button>().colors;
            colour.normalColor = unavaliableNormalColour;
            GetComponent<Button>().colors = colour;
        }
    }

    public void SaveFunction(object sender, EventArgs args)
    {
        SavedUpgrade savedUpgrade = new SavedUpgrade();
        savedUpgrade.ID = id;
        savedUpgrade.Avaliable = avaliable;
        savedUpgrade.Unlocked = unlocked;
        //print(skillPoint.PickedUp); this saves properly

        if(SaveLists.Instance.GetUpgradeList().SavedUpgradeSkills.Count == 0)
        {
            print(SaveLists.Instance.GetUpgradeList());
            SaveLists.Instance.GetUpgradeList().SavedUpgradeSkills.Add(savedUpgrade);
        }
        else
        {
            SaveLists.Instance.GetUpgradeList().SavedUpgradeSkills[0] = savedUpgrade;
        }   
    }

    public void IncreaseDaggerCombo()
    {
        player.dagger.GetComponent<Dagger>().curComboAmount++;
        //Debug.Log("Dagger combo amount is now: " + player.dagger.GetComponent<Dagger>().curComboAmount);

        for(int i = 0; i < upgradeScript.Length; i++)
        {
            Upgrade script = upgradeScript[i];
            script.avaliable = true;
            script.UpdateColour();
        }

        unlocked = true;
        avaliable = false;

        UpdateColour();
    }

    public void IncreaseHammerCombo()
    {
        player.hammer.GetComponent<Hammer>().curComboAmount++;
        //Debug.Log("Hammer combo amount is now: " + player.hammer.GetComponent<Hammer>().curComboAmount);

        for (int i = 0; i < upgradeScript.Length; i++)
        {
            Upgrade script = upgradeScript[i];
            script.avaliable = true;
            script.UpdateColour();
        }

        unlocked = true;
        avaliable = false;

        UpdateColour();
    }

    public void IncreaseSwordCombo()
    {
        player.sword.GetComponent<Sword>().curComboAmount++;
        //Debug.Log("Sword combo amount is now: " + player.sword.GetComponent<Sword>().curComboAmount);

        for (int i = 0; i < upgradeScript.Length; i++)
        {
            Upgrade script = upgradeScript[i];
            script.avaliable = true;
            script.UpdateColour();
        }

        unlocked = true;
        avaliable = false;

        UpdateColour();
    }

    public void IncreaseRodOfLightCombo()
    {
        player.rodOfLight.GetComponent<RodOfLight>().curComboAmount++;
        //Debug.Log("Rod Of Light combo amount is now: " + player.rodOfLight.GetComponent<RodOfLight>().curComboAmount);

        for (int i = 0; i < upgradeScript.Length; i++)
        {
            Upgrade script = upgradeScript[i];
            script.avaliable = true;
            script.UpdateColour();
        }

        unlocked = true;
        avaliable = false;

        UpdateColour();
    }

    public void IncreaseMaxHP(int Amount)
    {
        player.HP.MaxVal += Amount;
        //Debug.Log(" the players max HP is now: " + player.HP.MaxVal);

        for (int i = 0; i < upgradeScript.Length; i++)
        {
            Upgrade script = upgradeScript[i];
            script.avaliable = true;
            script.UpdateColour();
        }

        unlocked = true;
        avaliable = false;

        UpdateColour();
    }

    public void IncreaseMaxSp(int Amount)
    {
        player.SP.MaxVal += Amount;
       // Debug.Log(" the players max SP is now: " + player.SP.MaxVal);

        for (int i = 0; i < upgradeScript.Length; i++)
        {
            Upgrade script = upgradeScript[i];
            script.avaliable = true;
            script.UpdateColour();
        }

        unlocked = true;
        avaliable = false;

        UpdateColour();
    }
}
