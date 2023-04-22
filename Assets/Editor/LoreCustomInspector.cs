using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LoreSO))]
public class LoreCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        // Get a reference to the Scriptable Object instance
        var scriptableObject = (LoreSO)target;

        if (scriptableObject == null) return;
        
        serializedObject.Update();
        
        // Draw the default inspector
        DrawDefaultInspector();

        // Add a multi-line text area for the string field
        EditorGUILayout.LabelField("Pasting text with newlines:");

        var loreText = serializedObject.FindProperty("loreText");

        if (loreText != null)
        {
            string pastedText = EditorGUILayout.TextArea(loreText.stringValue ?? "");
            pastedText = pastedText.Replace("\r", string.Empty)
                .Replace("\n", System.Environment.NewLine)
                .Replace("\t", "    ");
            loreText.stringValue = pastedText;
        }

        serializedObject.ApplyModifiedProperties();

        /*scriptableObject.LoreText = EditorGUILayout.TextArea(scriptableObject.LoreText);

        scriptableObject.LoreText = scriptableObject.LoreText.Replace("\r", string.Empty)
            .Replace("\n", System.Environment.NewLine).Replace("\t", "     ");

        // Save any changes made in the custom inspector
        if (GUI.changed) EditorUtility.SetDirty(scriptableObject);*/
    }
}