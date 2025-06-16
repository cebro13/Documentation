using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public static class Loader
{
    public static event EventHandler<EventArgs> OnLoadNewScene;


    public static void ResetStaticData()
    {
        OnLoadNewScene = null;
    }

    public enum Scene
    {
        MainMenuScene,
        LoadingScene,
        ExteriorConspirationiste_Main,
        ConspirationScene_Main,
        ConspirationPipe_Main,
        ConspirationnisteBossScene_Main,
        VilleScene_Main,
        ForestJunkyard_Main,
        ForestPit_Main,
        ManoirScene_Main,
        NBScene,
        Last
    }

    private static Scene m_targetScene;

    public static void Load(Scene targetScene)
    {
        m_targetScene = targetScene;
        OnLoadNewScene?.Invoke(null, EventArgs.Empty);
        DataPersistantManager.Instance.SaveGame();
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
        }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(m_targetScene.ToString());
    }
}
