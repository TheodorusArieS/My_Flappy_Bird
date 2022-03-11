using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.SceneManagement;

public static class Loader
{
    private static Scene targetScene;
    public enum Scene
    {
        GameScene,
        LoadingScene,
        MainMenu
    }

    public static void Load(Scene scene)
    {
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        targetScene = scene;
    }

    public static void LoadingUpdate()
    {

        SceneManager.LoadScene(targetScene.ToString());
    }
}
