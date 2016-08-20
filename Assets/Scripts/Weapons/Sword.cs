using UnityEngine;
using System.Collections;

public class Sword : Weapon {

    public static Sword Instance;

    void Awake()
    {
        if (Instance == null)//checks to see if there is a game control object and if there isnt one, make this that game object
        {
            DontDestroyOnLoad(gameObject);//stop this game object from being deleted between scenes
            Instance = this;//makes this game object the game control object in use
        }
        else if (Instance != this)// checks to see if there is a game object, and if there is one but it is not this game object 
        {
            Destroy(gameObject);// then it destroys itself
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
