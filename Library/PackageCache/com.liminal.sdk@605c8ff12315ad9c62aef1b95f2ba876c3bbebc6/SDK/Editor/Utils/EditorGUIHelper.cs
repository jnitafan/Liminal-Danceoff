using UnityEditor;
using UnityEngine;

public static class EditorGUIHelper
{
    public static void DrawTitle(string label)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Height(30));
        EditorGUILayout.LabelField(label, EditorStyles.whiteLargeLabel);
        EditorGUILayout.EndVertical();
    }
}