using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelector : MonoBehaviour
{
    public List<SceneModel> scenes;

    public void LoadScene(int sceneID)
    {
        if (sceneID >= 0 && sceneID < scenes.Count)
        {
            SceneManager.LoadScene(sceneID);
        }
    }
}

[System.Serializable]
public class SceneModel
{
    public int sceneID;
    public string sceneName;
}
