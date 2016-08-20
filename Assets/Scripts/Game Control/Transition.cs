using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour {

    //which scene are we going to?
    public int targetSceneIndex;

    public Transform targetPlayerLocation;

    void Interact()
    {
        GameControl.Instance.transitionTarget.position = targetPlayerLocation.position;

        GameControl.Instance.isSceneBeingTransitioned = true;
        SaveLists.Instance.FireSaveEvent();

        SceneManager.LoadScene(targetSceneIndex);
    }
}
