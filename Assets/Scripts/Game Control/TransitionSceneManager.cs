using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransitionSceneManager : MonoBehaviour {

    public static TransitionSceneManager Instance;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator AsynchronusLoad(int sceneBuildNumber)
    {
        yield return null;

        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneBuildNumber);
        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            //[0, 0.9] > [0,1] this equation only goes to 0.9, not to 1
            float progress = Mathf.Clamp01(ao.progress / 0.9f);
            Debug.Log("Loading Progress: " + (progress * 100) + "%");

            //loading complete
            if (ao.progress == 0.9f)
            {
                Debug.Log("Press the X button or Left Click to Start");
                if (Input.GetAxis("Controller Jump") == 1 || Input.GetAxis("Keyboard Jump") == 1)
                {
                    ao.allowSceneActivation = true;
                    //  Destroy(gameObject);
                }
            }

            yield return null;
        }
    }
}
