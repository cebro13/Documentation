using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button m_playNBSceneButton;
    [SerializeField] private Button m_continueNBSceneButton;
    [SerializeField] private Button m_playCEBSceneButton;
    [SerializeField] private Button m_quitButton;

    private GameObject m_lastSelectedButton;

    private void Awake()
    {

    }

    private void Start()
    {
        m_playNBSceneButton.onClick.AddListener(() => {
            DataPersistantManager.Instance.NewGame();
            Loader.Load(Loader.Scene.ExteriorConspirationiste_Main);
        });
        
        if(!DataPersistantManager.Instance.HasGameData())
        {
            m_continueNBSceneButton.interactable = false;
        }

        m_continueNBSceneButton.onClick.AddListener(() => {
            //Click Lambda function
            Loader.Load(DataPersistantManager.Instance.GetLastSceneIn());
        });

        m_playCEBSceneButton.onClick.AddListener(() => {
            //Click Lambda function
            DataPersistantManager.Instance.NewGame();
            Loader.Load(Loader.Scene.VilleScene_Main);
        });

        m_quitButton.onClick.AddListener(() => {
            //Click Lambda function
            Application.Quit();
        });

        Time.timeScale = 1f;
    }
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(m_lastSelectedButton);
        }
        else
        {
            m_lastSelectedButton = EventSystem.current.currentSelectedGameObject;
        }
    }
}
