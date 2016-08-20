using UnityEngine;
using System.Collections.Generic;

public class ObjectLists : MonoBehaviour {

    public static ObjectLists Instance;

    public List<GameObject> obsticalList = new List<GameObject>();
    public List<GameObject> itemList = new List<GameObject>();

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

        obsticalList.Clear();
        itemList.Clear();
    }
}
