using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public void SceneLoad(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
