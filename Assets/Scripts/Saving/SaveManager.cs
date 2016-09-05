using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {

    public void SaveGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        PlayerState.Instance.localPlayerData.SceneID = scene.buildIndex;
        PlayerState.Instance.localPlayerData.PosX = transform.position.x;
        PlayerState.Instance.localPlayerData.PosY = transform.position.y;

        SaveLists.Instance.SaveData();
    }

    public void LoadGame()
    {
        SaveLists.Instance.LoadData();
        GameControl.Instance.isSceneBeingLoaded = true;

        int whichScene = SaveLists.Instance.localCopyOfData.SceneID;

        SceneManager.LoadScene(whichScene);
    }
}
