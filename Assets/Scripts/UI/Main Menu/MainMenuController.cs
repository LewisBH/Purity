using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    private SaveManager saveManager;

	// Use this for initialization
	void Start () {
        saveManager = GetComponent<SaveManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void NewGame()
    {
        StartCoroutine(TransitionSceneManager.Instance.AsynchronusLoad(1));
    }

    public void LoadGame()
    {
        SceneManager.LoadSceneAsync(1);
        saveManager.LoadGame();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
