using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    public static ScenesManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance is null) {
            Instance = this;
        }
    }

    public enum Scene
    {
        Demo
    }

    public void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioManager.Instance.StopBGM();
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
