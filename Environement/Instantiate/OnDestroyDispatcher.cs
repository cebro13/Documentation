using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnDestroyDispatcher : MonoBehaviour
{
    public event EventHandler<EventArgs> OnObjectDestroy;
    private bool m_isQuitting;
    private bool m_loadNewScene;

    private void Awake()
    {
        m_isQuitting = false;
        m_loadNewScene = false;
    }

    private void Start()
    {
        Loader.OnLoadNewScene += Loader_OnLoadNewScene;
    }

    private void Loader_OnLoadNewScene(object sender, EventArgs e)
    {
        m_loadNewScene = true;
    }

    private void OnApplicationQuit()
    {
        m_isQuitting = true;
    }

    private void OnDestroy()
    {
        if(!m_isQuitting && !m_loadNewScene)
        {
            OnObjectDestroy?.Invoke(this, EventArgs.Empty);
        }
    }

}
