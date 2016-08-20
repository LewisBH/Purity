using UnityEngine;
using System.Collections;
using System;

public class Boss : Enemy {

    public bool isDead;
    public Obstical obstical;
	
    // Use this for initialization
	void Start () {
        SaveLists.SaveEvent += BossSaveFunction;
	}

    void BossSaveFunction(object sender, EventArgs args)
    {
        //print("boss save event called");

        SavedBoss boss = new SavedBoss();
        boss.isDead = isDead;
        boss.obstical = obstical;
        boss.name = gameObject.name;

        print(gameObject.name + " " + isDead);
        SaveLists.Instance.GetEnemyListForScene().savedBosses.Add(boss);
    }
	
}
