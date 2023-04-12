using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoreSO))]
public class LoreCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        // Get a reference to the Scriptable Object instance
        var scriptableObject = (LoreSO)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Add a multi-line text area for the string field
        EditorGUILayout.LabelField("Pasting text with newlines:");
        scriptableObject.LoreText = EditorGUILayout.TextArea(scriptableObject.LoreText);

        scriptableObject.LoreText = scriptableObject.LoreText.Replace("\r", string.Empty)
            .Replace("\n", System.Environment.NewLine).Replace("\t", "     ");

        // Save any changes made in the custom inspector
        if (GUI.changed) EditorUtility.SetDirty(target);
    }
}