using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class AutoSave
{
    // Static constructor that gets called when unity fires up.
    static AutoSave()
    {
#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) => {
            // If we're about to run the scene...
            if (EditorApplication.isPlayingOrWillChangePlaymode && !EditorApplication.isPlaying)
            {
                // Save the scene and the assets.
                Debug.Log("Auto-saving all open scenes... " + state);
                EditorSceneManager.SaveOpenScenes();
                AssetDatabase.SaveAssets();
            }
        };
#endif
    }
}
