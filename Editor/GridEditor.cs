using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    Grid grid;

    public void OnEnable()
    {
        grid = target as Grid;

        SceneView.onSceneGUIDelegate -= GridUpdate;// this removes the delegate

        SceneView.onSceneGUIDelegate += GridUpdate;// this allows us to grab the mouse states. a delegate is a reference to a method (function)
    }

    public void OnDisable()
    {
        
    }

    void GridUpdate(SceneView sceneView)
    {
        Event e = Event.current;

        Ray r = Camera.current.ScreenPointToRay(new Vector3(e.mousePosition.x, -e.mousePosition.y + Camera.current.pixelHeight));
        Vector3 mousePos = r.origin;

        if(e.isKey && e.character == 'a')
        {
            GameObject obj;
            Object prefab = PrefabUtility.GetPrefabParent(Selection.activeObject);//gets the selected object as a prefab

            if(prefab)//selection.activeObject is a reference to the currently selected object in the editor
            {
                obj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);//creates an object as a prefab and not a clone.
                Vector3 aligned = new Vector3(Mathf.Floor(mousePos.x / grid.width) * grid.width + grid.width / 2.0f,
                                              Mathf.Floor(mousePos.y / grid.height) * grid.height + grid.height / 2.0f, 0.0f);

                obj.transform.position = aligned;
                Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            }
        }
        else if(e.isKey && e.character == 'd')
        {
            foreach (GameObject obj in Selection.gameObjects)
            {
                DestroyImmediate(obj);
            } 
        }
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();// this starts placing the elements to the right of each other.
        GUILayout.Label(" Grid Width ");
        grid.width = EditorGUILayout.FloatField(grid.width, GUILayout.Width(50));// creats a feild in the inspector that requires a float

        if(grid.width < 0.1f)
        {
            grid.width = 0.1f;
        }

        GUILayout.EndHorizontal();//stops placing elements next to each other.

        GUILayout.BeginHorizontal();// this starts placing the elements to the right of each other.
        GUILayout.Label(" Grid Height ");
        grid.height = EditorGUILayout.FloatField(grid.width, GUILayout.Width(50));// creats a feild in the inspector that requires a float

        if (grid.height < 0.1f)
        {
            grid.height = 0.1f;
        }

        GUILayout.EndHorizontal();//stops placing elements next to each other.

        if(GUILayout.Button("Open Grid Window", GUILayout.Width(255)))
        {
            GridWindow window = (GridWindow)EditorWindow.GetWindow(typeof(GridWindow));
            window.Init();
        }

        SceneView.RepaintAll();
    }

}
