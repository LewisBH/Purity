using UnityEngine;
using System.Collections;
using System;

public class Enemy : MonoBehaviour {

    public GameObject player;
    public int maxHealth;
    public int curHealth;
    public float moveSpeed;
    public float attackSpeed;
    public int touchDamage;
    public int expDropAmount;

    //TO DO  create lists for the walking, attacking and all the other kind of animations

	// Use this for initialization
	void Start ()
    {
        player = Player.Instance.gameObject;

        curHealth = maxHealth;
    }
}