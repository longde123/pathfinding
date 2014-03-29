using UnityEngine;
using UnityEditor;
using System.Collections;
[CustomEditor (typeof (Grid))]
public class GridEditor : Editor {

	public override void OnInspectorGUI () {
		Grid grid = target as Grid;
		
		EditorGUILayout.BeginHorizontal();
		grid.Unit = EditorGUILayout.IntField("Unit", grid.Unit);
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		grid.GridX = EditorGUILayout.IntField("X Offset", grid.GridX);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		grid.GridY = EditorGUILayout.IntField("Z Offset", grid.GridY);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		grid.GridWidth = EditorGUILayout.IntField("Grid Width", grid.GridWidth);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		grid.GridLength = EditorGUILayout.IntField("Grid Lenght", grid.GridLength);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		grid.GridHeight = EditorGUILayout.IntField("Grid Height", grid.GridHeight);
		EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.BeginHorizontal();
		grid.ShowUnwalkables = EditorGUILayout.Toggle("Show Unwalkables", grid.ShowUnwalkables);
		EditorGUILayout.EndHorizontal();
			
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Scan")){
			grid.Scan();
		}
		EditorGUILayout.EndHorizontal();

		if(GUI.changed)
			EditorUtility.SetDirty(grid);
	}
}
