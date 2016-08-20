using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState : MonoBehaviour {

    public static PlayerState Instance;

    public Transform playerPosition;

    public PlayerStatistics localPlayerData = new PlayerStatistics();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        if(Instance != this)
        {
            Destroy(gameObject);
        }
    }
	// Use this for initialization
	void Start () {
        if(SaveLists.Instance.savedPlayerData == null)
        {
            Debug.LogError("Tried to get player data, but it doesn't exist");
            
        }
        else
        {
            localPlayerData = SaveLists.Instance.savedPlayerData;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
