using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneDataPersistant : MonoBehaviour
{
    public static event EventHandler<OnSceneDataPersistantTriggeredEventArgs> OnNewSceneDataPersistant;
    public class OnSceneDataPersistantTriggeredEventArgs : EventArgs
    {
        public int sceneDataPersistantNumber;
        public bool saveGame;
    }

    [SerializeField] private List<GameObject> m_switchableGameObjectsOnLoad;
    [SerializeField] private List<GameObject> m_setActiveGameObjectsOnLoad;
    [SerializeField] private List<ObjectToMoveOnLoad> m_moveGameObjectsOnLoad;

    private List<ISwitchable> m_switchableOnLoad;

    [SerializeField] private int m_sceneDataPersistantNumber;

    public static void ResetStaticData()
    {
        OnNewSceneDataPersistant = null;
    }

    private void Awake()
    {
        m_switchableOnLoad = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjectsOnLoad)
        {
            ISwitchable iSwitchable = switchableGameObject.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchableOnLoad.Add(iSwitchable);
        }
    }

    public void SetNewSceneDataPersistant(bool isSaveGame = true)
    {
        OnNewSceneDataPersistant?.Invoke(this, new OnSceneDataPersistantTriggeredEventArgs{
            sceneDataPersistantNumber = m_sceneDataPersistantNumber,
            saveGame = isSaveGame
        });
    }

    public void SwitchAllOnLoad()
    {
        foreach(ISwitchable iSwitchable in m_switchableOnLoad)
        {
            iSwitchable.Switch();
        }
        foreach(GameObject gameObject in m_setActiveGameObjectsOnLoad)
        {
            if(gameObject.activeSelf)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
            }
        }
        
        foreach(ObjectToMoveOnLoad objectToMove in m_moveGameObjectsOnLoad)
        {
            objectToMove.transformObject.position = objectToMove.position;
        }
    }

    public int GetSceneDataPersistantNumber()
    {
        return m_sceneDataPersistantNumber;
    }

    [Serializable]
    public class ObjectToMoveOnLoad
    {
        public Transform transformObject;
        public Vector2 position;
    }


}
