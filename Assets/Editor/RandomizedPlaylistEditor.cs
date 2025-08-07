using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomizedVideoPlaylist))]
public class RandomizedPlaylistEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space(10); // Adds some visual separation

        RandomizedVideoPlaylist playlist = (RandomizedVideoPlaylist)target;

        // --- BUTTON 1: FIX ALPHA (from before) ---
        if (GUILayout.Button("Set All Theme Colors to Opaque"))
        {
            if (EditorUtility.DisplayDialog("Confirm Action", 
                "Are you sure you want to set the alpha to 255 for all theme colors in this playlist? This action cannot be undone.", 
                "Yes, Fix It", "Cancel"))
            {
                FixAlphaValues(playlist);
            }
        }
        
        EditorGUILayout.Space(5); // More separation

        // --- BUTTON 2: SWAP SPECIFIC COLORS (the new one) ---
        if (GUILayout.Button("Swap Colors for a Specific Theme"))
        {
            if (EditorUtility.DisplayDialog("Confirm Color Swap", 
                "This will find and replace colors in a specific theme based on the hardcoded values in the 'ThemeColorSwapEditor.cs' script. Are you sure?",
                "Yes, Swap Colors", "Cancel"))
            {
                // This calls the logic from your new script
                ThemeColorSwapEditor.SwapThemeColors(playlist);
            }
        }
    }

    private void FixAlphaValues(RandomizedVideoPlaylist playlist)
    {
        Undo.RecordObject(playlist, "Set All Theme Colors to Opaque");
        SerializedObject serializedPlaylist = new SerializedObject(playlist);
        SerializedProperty videoThemesProperty = serializedPlaylist.FindProperty("videoThemes");

        for (int i = 0; i < videoThemesProperty.arraySize; i++)
        {
            SerializedProperty themeProperty = videoThemesProperty.GetArrayElementAtIndex(i);
            SerializedProperty colorTargetsProperty = themeProperty.FindPropertyRelative("colorTargets");

            for (int j = 0; j < colorTargetsProperty.arraySize; j++)
            {
                SerializedProperty colorTargetProperty = colorTargetsProperty.GetArrayElementAtIndex(j);
                SerializedProperty colorProperty = colorTargetProperty.FindPropertyRelative("themeColor");

                Color currentColor = colorProperty.colorValue;
                currentColor.a = 1.0f;
                colorProperty.colorValue = currentColor;
            }
        }

        serializedPlaylist.ApplyModifiedProperties();
        Debug.Log("All theme color alphas have been set to 255 (opaque).");
    }
}