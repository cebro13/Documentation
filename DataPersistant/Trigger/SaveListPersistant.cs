using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ShowAttribute;

public class SaveListPersistant : MonoBehaviour, ICanInteract, ISwitchable, IDataPersistant
{
    [Header("On peut se servir de Set On Trigger ET Set On Load si on veut")]
    [SerializeField] private string m_ID;
    [Header("Si DataParsistant n'est pas active, on se sert de ce component comme switch on trigger")]
    [SerializeField] private bool m_isDataPersistantActive = true;
    [SerializeField] private Utils.TriggerType m_triggerType = Utils.TriggerType.Switch;

    [Header("Set On Trigger Only")]
    [SerializeField] private bool m_isSwitchOnTrigger = false;
    [SerializeField] private bool m_isTriggerOnceOnly = false;
    [SerializeField] private bool m_isSaveGameOnTrigger = false;
    [SerializeField] private List<GameObject> m_swapActiveOnTrigger;
    [SerializeField] private List<ObjectToSetActive> m_setActiveOnTrigger;
    [SerializeField] private List<AnimatorToTrigger> m_triggerAnimatorOnTrigger;
    [SerializeField] private List<GameObject> m_switchableGameObjectsOnTrigger;
    [SerializeField] private List<ObjectToSendEvent> m_toSendEventOnTrigger;

    [Header("Set On Load Only")]
    [SerializeField] private List<ObjectToSetActive> m_setActiveOnLoad;
    [SerializeField] private List<AnimatorToTrigger> m_triggerAnimatorOnLoad;
    [SerializeField] private List<ObjectToSendEvent> m_toSendEventOnLoad;
    [SerializeField] private List<GameObject> m_switchableGameObjectsOnLoad;

    [Header("Debugger")]
    [SerializeField] private bool m_testSwitchOnTrigger = false;
    
    private List<ISwitchable> m_switchableOnTrigger;
    private List<ISwitchable> m_switchableOnLoad;

    private bool m_hasBeenActivate;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_hasBeenActivate = false;
        m_switchableOnTrigger = new List<ISwitchable>();
        foreach(GameObject switchableGameObject in m_switchableGameObjectsOnTrigger)
        {
            ISwitchable iSwitchable = switchableGameObject.GetComponent<ISwitchable>();
            if(iSwitchable == null)
            {
                Debug.LogError("GameObjet" + switchableGameObject + " does not have a component that implements ISwitchable");
            }
            m_switchableOnTrigger.Add(iSwitchable);
        }

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

    public void LoadData(GameData data)
    {
        data.switchAfterLoad.TryGetValue(m_ID, out m_hasBeenActivate);
        if(m_hasBeenActivate && m_isDataPersistantActive)
        {
            SwitchOnLoad();
        }
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantActive)
        {
            return;
        }
        if(data.switchAfterLoad.ContainsKey(m_ID))
        {
            data.switchAfterLoad.Remove(m_ID);
        }
        data.switchAfterLoad.Add(m_ID, m_hasBeenActivate);
    }

    public void Save()
    {
        DataPersistantManager.Instance.SaveGame();
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        SwitchOnTrigger();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        SwitchOnTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }
        if(m_triggerType == Utils.TriggerType.ColliderEnter || m_triggerType == Utils.TriggerType.ColliderEnterAndExit)
        {
            SwitchOnTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer != Player.PLAYER_LAYER)
        {
            return;
        }

        if(m_triggerType == Utils.TriggerType.ColliderEnterAndExit || m_triggerType == Utils.TriggerType.ColliderExit)
        {
            SwitchOnTrigger();
        }
    }

    private void SwitchOnTrigger()
    {
        if(m_hasBeenActivate && m_isTriggerOnceOnly)
        {
            return;
        }
        
        if(!m_hasBeenActivate)
        {
            m_hasBeenActivate = true;
        }

        if(!m_isSwitchOnTrigger)
        {
            return;
        }

        foreach(GameObject gameObjectToSwap in m_swapActiveOnTrigger)
        {
            Debug.Log("Ici");
            Debug.Log(gameObjectToSwap.name);
            gameObjectToSwap.SetActive(!gameObjectToSwap.activeSelf);
        }
        foreach(ISwitchable iSwitchable in m_switchableOnTrigger)
        {
            iSwitchable.Switch();
        }
        foreach(AnimatorToTrigger animatorToTrigger in m_triggerAnimatorOnTrigger)
        {
            if(animatorToTrigger.gameObjectWithAnimator.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger(animatorToTrigger.animatorTriggerString);
            }
            else
            {
                Debug.LogError("L'objet " + animatorToTrigger.gameObjectWithAnimator.name +  " que vous avez ajoutez à la SaveListPersistant ne contient pas de component Animator");
            }
        }
        foreach(ObjectToSetActive objectToSetActive in m_setActiveOnTrigger)
        {
            objectToSetActive.gameObjectToSetActive.SetActive(objectToSetActive.isActive);
        }
        foreach(ObjectToSendEvent objectToSendEvent in m_toSendEventOnTrigger)
        {
            CustomEvent.Trigger(objectToSendEvent.gameObjectWithVisualScript, objectToSendEvent.eventString);
        }
        
        if(m_isSaveGameOnTrigger)
        {
            Save();
        }
    }

    private void SwitchOnLoad()
    {
        foreach(ISwitchable iSwitchable in m_switchableOnLoad)
        {
            iSwitchable.Switch();
        }
        foreach(AnimatorToTrigger animatorToTrigger in m_triggerAnimatorOnLoad)
        {
            if(animatorToTrigger.gameObjectWithAnimator.TryGetComponent(out Animator animator))
            {
                animator.SetTrigger(animatorToTrigger.animatorTriggerString);
            }
            else
            {
                Debug.LogError("L'objet " + animatorToTrigger.gameObjectWithAnimator.name +  " que vous avez ajoutez à la SaveListPersistant ne contient pas de component Animator");
            }
        }
        foreach(ObjectToSetActive objectToSetActive in m_setActiveOnLoad)
        {
            objectToSetActive.gameObjectToSetActive.SetActive(objectToSetActive.isActive);
        }
        foreach(ObjectToSendEvent objectToSendEvent in m_toSendEventOnLoad)
        {
            CustomEvent.Trigger(objectToSendEvent.gameObjectWithVisualScript, objectToSendEvent.eventString);
        }
    }

    private void Update()
    {
        if(m_testSwitchOnTrigger)
        {
            SwitchOnTrigger();
            m_testSwitchOnTrigger = false;
        }
    }

    [Serializable]
    public class AnimatorToTrigger
    {
        public GameObject gameObjectWithAnimator;
        public string animatorTriggerString;
    }

    [Serializable]
    private class ObjectToSendEvent
    {
        public GameObject gameObjectWithVisualScript;
        public string eventString;
    }

    [Serializable]
    private class ObjectToSetActive
    {
        public GameObject gameObjectToSetActive;
        public bool isActive;
    }
}
