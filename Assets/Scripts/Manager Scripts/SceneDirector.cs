using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneDirector : MonoBehaviour
{
    void Update()
    {
    }

    public void UnloadScene(string _sceneName)
    {
        SceneManager.UnloadScene(_sceneName);
    }

    public void LoadScene(string _sceneName)
    {
        SceneManager.LoadScene(_sceneName, LoadSceneMode.Additive);
    }

    public void QuitRequest()
    {
        Application.Quit();
    }
}