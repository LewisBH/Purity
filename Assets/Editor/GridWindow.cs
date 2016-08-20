using UnityEngine;
using UnityEditor;
using System.Collections;

public class GridWindow : EditorWindow {

    Grid grid;

	public void Init()
    {
        grid = (Grid)FindObjectOfType(typeof(Grid));
    }

    void OnGUI()
    {
        grid.colour = EditorGUILayout.ColorField(grid.colour, GUILayout.Width(200));
    }
}
