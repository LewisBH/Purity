using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerStatistics{

    public int SceneID;
    public int SkillPoints;

    public float PosX, PosY;

    public Stat HP;
    public Stat SP;
}

[Serializable]
public class SavedPickUpableSkillPoint
{
    public bool PickedUp;
}

[Serializable]
public class SavedItemList // this class is what stores all of the items in a list
{
    public int SceneID;
    public List<SavedPickUpableSkillPoint> SavedSkillPoints;

    public SavedItemList(int newSceneID)
    {
        SceneID = newSceneID;
        SavedSkillPoints = new List<SavedPickUpableSkillPoint>();
    }
}

[Serializable]
public class SavedBoss
{
    public bool isDead;
    public Obstical obstical;
    public string name;
}

[Serializable]
public class SavedEnemyList// this stores all the enemies in a list
{ 
    public int SceneID;
    public List<SavedBoss> savedBosses;

    public SavedEnemyList(int newSceneID)
    {
        SceneID = newSceneID;
        savedBosses = new List<SavedBoss>();
    }
}

[Serializable]
public class SavedUpgrade
{
    public int ID;
    public bool Avaliable;
    public bool Unlocked;
}

[Serializable]
public class SavedUpgrades
{
    public List<SavedUpgrade> SavedUpgradeSkills;

    public SavedUpgrades()
    {
        SavedUpgradeSkills = new List<SavedUpgrade>();
    }
}

[Serializable]
public class SavedObstical
{
    public Obstical.state curState;
    public string name;
}

[Serializable]
public class SavedObsticalList
{
    public int SceneID;
    public List<SavedObstical> savedObsticals;

    public SavedObsticalList(int newSceneID)
    {
        SceneID = newSceneID;
        savedObsticals = new List<SavedObstical>();
    }
}