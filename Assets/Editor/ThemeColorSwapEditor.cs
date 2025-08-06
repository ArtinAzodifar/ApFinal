using UnityEngine;
using UnityEditor;

public class ThemeColorSwapEditor
{
    // --- SET TO TRUE TO SEE EXACT COLOR VALUES IN THE CONSOLE ---
    private const bool DEBUG_MODE = false;

    public static void SwapThemeColors(RandomizedVideoPlaylist playlist)
    {
        int targetThemeIndex = 3; 

        // Set the four HEX colors for your find-and-replace operation.
        string originalDarkHex  = "#483B37";
        string newDarkHex       = "#292D3F";
        string originalLightHex = "#E1CFAF";
        string newLightHex      = "#A9B1D6";

        SerializedObject serializedPlaylist = new SerializedObject(playlist);
        SerializedProperty videoThemesProperty = serializedPlaylist.FindProperty("videoThemes");

        if (targetThemeIndex < 0 || targetThemeIndex >= videoThemesProperty.arraySize)
        {
            Debug.LogError($"Invalid Target Theme Index: {targetThemeIndex}. The playlist only has {videoThemesProperty.arraySize} elements. Action cancelled.");
            return;
        }

        if (!ColorUtility.TryParseHtmlString(originalDarkHex, out Color originalDarkColor) ||
            !ColorUtility.TryParseHtmlString(newDarkHex, out Color newDarkColor) ||
            !ColorUtility.TryParseHtmlString(originalLightHex, out Color originalLightColor) ||
            !ColorUtility.TryParseHtmlString(newLightHex, out Color newLightColor))
        {
            Debug.LogError("Failed to parse one or more HEX color codes. Please check the strings. Action cancelled.");
            return;
        }

        int changesMade = 0;
        Undo.RecordObject(playlist, "Swap Theme Colors");
        
        SerializedProperty themeProperty = videoThemesProperty.GetArrayElementAtIndex(targetThemeIndex);
        SerializedProperty colorTargetsProperty = themeProperty.FindPropertyRelative("colorTargets");

        if (DEBUG_MODE) Debug.Log($"--- Starting Color Swap for Theme {targetThemeIndex + 1} ---");

        for (int i = 0; i < colorTargetsProperty.arraySize; i++)
        {
            SerializedProperty colorProperty = colorTargetsProperty.GetArrayElementAtIndex(i).FindPropertyRelative("themeColor");
            Color currentColor = colorProperty.colorValue;

            if (DEBUG_MODE)
            {
                Debug.Log($"Checking element {i}: Color is {ColorUtility.ToHtmlStringRGBA(currentColor)} ({currentColor.ToString("F8")})");
            }

            if (AreColorsSimilar(currentColor, originalDarkColor))
            {
                colorProperty.colorValue = newDarkColor;
                changesMade++;
            }
            else if (AreColorsSimilar(currentColor, originalLightColor))
            {
                colorProperty.colorValue = newLightColor;
                changesMade++;
            }
        }

        if (changesMade > 0)
        {
            serializedPlaylist.ApplyModifiedProperties();
            Debug.Log($"Color swap complete for theme element {targetThemeIndex + 1}. {changesMade} colors were changed.");
        }
        else
        {
            Debug.Log($"No colors matched the original HEX codes in theme element {targetThemeIndex + 1}. No changes were made.");
        }
    }
    
    // Helper function to compare colors with a tolerance
    private static bool AreColorsSimilar(Color c1, Color c2)
    {
        // A small tolerance to account for floating point inaccuracies
        float tolerance = 0.01f; 
        
        return Mathf.Abs(c1.r - c2.r) < tolerance &&
               Mathf.Abs(c1.g - c2.g) < tolerance &&
               Mathf.Abs(c1.b - c2.b) < tolerance &&
               Mathf.Abs(c1.a - c2.a) < tolerance;
    }
}