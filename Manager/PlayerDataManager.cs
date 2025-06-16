using System;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour, IDataPersistant
{
    public event EventHandler<OnNewPowerFoundEventArg> OnNewPowerFound;
    public class OnNewPowerFoundEventArg : EventArgs
    {
        public PowerUp powerUp;
    }

    public event EventHandler<OnNewFoundKnowledgeEventArgs> OnNewFoundKnowledge;
    public class OnNewFoundKnowledgeEventArgs : EventArgs
    {
        public KnowledgeUI.eKnowledgeUI knowledgeUiIdArg;
    }

    public event EventHandler<OnNewFoundItemEventArgs> OnNewItemFound;
    public class OnNewFoundItemEventArgs : EventArgs
    {
        public ItemUI.eItemUI itemUiIdArg;
    }

    public event EventHandler<OnNewFoundBusStopEventArgs> OnNewBusStopFound;
    public class OnNewFoundBusStopEventArgs : EventArgs
    {
        public BusStopUI.eBusStop busStopArg;
    }

    public event EventHandler<EventArgs> OnIncreaseHauntableDistance;

    public static PlayerDataManager Instance {get; private set;}

    public bool m_powerCanDash {get; private set;}
    public bool m_powerCanFly {get; private set;}
    public bool m_powerCanHaunt {get; private set;}
    public bool m_powerCanFear {get; private set;}

    public int m_powerHauntDistance { get; private set; }

    public bool m_isFirstTimeNewFoundKnowledge {get; private set;}
    public bool m_newFoundKnowledgeMicrowaveTimer {get; private set;}
    public bool m_newFoundKnowledgeNext {get; private set;}
    public bool m_newFoundKnowledgeLast {get; private set;}

    public bool m_isFirstTimeItemFound{get; private set;}
    public bool m_isObjectConspirationnisteWifeRingFound {get; private set;}
    public bool m_isObjectAnOldKeyFound {get; private set;}
    public bool m_isCorrectBookFound {get; private set;}
    public bool m_isIncorrectBookFound {get; private set;}
    public bool m_isObjectLastObjectFound {get; private set;}

    public bool m_isBusStopVilleFound {get; private set;}
    public bool m_isBusStopExteriorConspiFound {get; private set;}
    public bool m_isBusStopLastFound {get; private set;}
    

    private void Awake()
    {
        Instance = this;
    }
    
    public void LoadData(GameData data)
    {
        m_powerCanDash = data.powerCanDash;
        m_powerCanFly = data.powerCanFly;
        m_powerCanHaunt = data.powerCanHaunt;
        m_powerCanFear = data.powerCanFear;

        m_powerHauntDistance = data.powerHauntDistance;

        m_isFirstTimeNewFoundKnowledge = data.firstTimeNewFoundKnowledge;
        m_newFoundKnowledgeMicrowaveTimer = data.newFoundKnowledgeMicrowaveTimerUnlocked;
        m_newFoundKnowledgeNext = data.newFoundKnowledgeNextUnlocked;
        m_newFoundKnowledgeLast = data.newFoundKnowledgeLastUnlocked;

        m_isFirstTimeItemFound = data.firstTimeItemFound;
        m_isObjectConspirationnisteWifeRingFound = data.isObjectConspirationnisteWifeRingFound;
        m_isObjectAnOldKeyFound = data.isObjectAnOldKeyFound;
        m_isCorrectBookFound = data.isCorrectBookFound;
        m_isIncorrectBookFound = data.isIncorrectBookFound;
        m_isObjectLastObjectFound = data.isObjectLastObjectFound;

        m_isBusStopVilleFound = data.isVilleBusStopFound;
        m_isBusStopExteriorConspiFound = data.isExteriorConspiBusStopFound;
        m_isBusStopLastFound = data.isLastBusStopFound;
    }

    public void SaveData(GameData data)
    {
        data.powerCanDash = m_powerCanDash;
        data.powerCanFly = m_powerCanFly;
        data.powerCanHaunt = m_powerCanHaunt;
        data.powerCanFear = m_powerCanFear;

        data.powerHauntDistance = m_powerHauntDistance;

        data.firstTimeNewFoundKnowledge = m_isFirstTimeNewFoundKnowledge;
        data.newFoundKnowledgeMicrowaveTimerUnlocked = m_newFoundKnowledgeMicrowaveTimer;
        data.newFoundKnowledgeNextUnlocked = m_newFoundKnowledgeNext;
        data.newFoundKnowledgeLastUnlocked = m_newFoundKnowledgeLast;

        data.firstTimeItemFound = m_isFirstTimeItemFound;
        data.isObjectConspirationnisteWifeRingFound = m_isObjectConspirationnisteWifeRingFound;
        data.isObjectAnOldKeyFound = m_isObjectAnOldKeyFound;
        data.isCorrectBookFound = m_isCorrectBookFound;
        data.isIncorrectBookFound = m_isIncorrectBookFound;
        data.isObjectLastObjectFound = m_isObjectLastObjectFound;

        data.isVilleBusStopFound = m_isBusStopVilleFound;
        data.isExteriorConspiBusStopFound = m_isBusStopExteriorConspiFound;
        data.isLastBusStopFound = m_isBusStopLastFound;
    }

    public void NewPowerUp(PowerUp newPowerUp)
    {
        switch(newPowerUp)
        {
            case PowerUp.CanDash:
                m_powerCanDash = true;
                break;
            case PowerUp.CanFly:
                m_powerCanFly = true;
                break;
            case PowerUp.CanHaunt:
                m_powerCanHaunt = true;
                break;
            case PowerUp.CanFear:
                m_powerCanFear = true;
                break;
            default:
                Debug.LogError("Ce cas ne devrait pas arriver");
                break;
        }

        OnNewPowerFound?.Invoke(this, new OnNewPowerFoundEventArg { powerUp = newPowerUp });
        DataPersistantManager.Instance.SaveGame();
    }

    public void NewFoundKnowledge(KnowledgeUI.eKnowledgeUI knowledgeUiID)
    {
        if(m_isFirstTimeNewFoundKnowledge)
        {
            m_isFirstTimeNewFoundKnowledge = false;
        }

        switch(knowledgeUiID)
        {
            case KnowledgeUI.eKnowledgeUI.MicrowaveTimer:
                m_newFoundKnowledgeMicrowaveTimer = true;
                break;
            case KnowledgeUI.eKnowledgeUI.SecondKnowledge:
                m_newFoundKnowledgeNext = true;
                break;
            case KnowledgeUI.eKnowledgeUI.LastKnowledge:
                m_newFoundKnowledgeLast = true;
                break;
            default:
                Debug.LogError("Ce cas ne devrait pas arriver");
                break;
        }

        OnNewFoundKnowledge?.Invoke(this, new OnNewFoundKnowledgeEventArgs{knowledgeUiIdArg = knowledgeUiID});
        
        DataPersistantManager.Instance.SaveGame();
    }

    public void NewItemFound(ItemUI.eItemUI itemUiID, bool skipFirstItemFoundCheck = false)
    {
        if(m_isFirstTimeItemFound && !skipFirstItemFoundCheck)
        {
            m_isFirstTimeItemFound = false;
        }

        switch(itemUiID)
        {
            case ItemUI.eItemUI.ConspirationnisteWifeRing:
                m_isObjectConspirationnisteWifeRingFound = true;
                break;
            case ItemUI.eItemUI.AnOldKey:
                m_isObjectAnOldKeyFound = true;
                break;
            case ItemUI.eItemUI.IncorrectBook:
                m_isIncorrectBookFound = true;
                break;
            case ItemUI.eItemUI.CorrectBook:
                m_isCorrectBookFound = true;
                break;
            case ItemUI.eItemUI.LastObject:
                m_isObjectLastObjectFound = true;
                break;
            default:
                Debug.LogError("Ce cas ne devrait pas arriver");
                break;
        }

        OnNewItemFound?.Invoke(this, new OnNewFoundItemEventArgs{itemUiIdArg = itemUiID});
        
        DataPersistantManager.Instance.SaveGame();
    }

    public void NewBusStopFound(BusStopUI.eBusStop busStop)
    {
        switch(busStop)
        {
            case BusStopUI.eBusStop.Ville_1000:
                m_isBusStopVilleFound = true;
                break;
            case BusStopUI.eBusStop.ExteriorConspirationniste_1000:
                m_isBusStopExteriorConspiFound = true;
                break;
            case BusStopUI.eBusStop.Last_1000:
                m_isBusStopLastFound = true;
                break;
            default:
                Debug.LogError("Ce cas ne devrait pas arriver");
                break;
        }

        OnNewBusStopFound?.Invoke(this, new OnNewFoundBusStopEventArgs{busStopArg = busStop});
        
        DataPersistantManager.Instance.SaveGame();
    }

    public void IncreaseHauntableDistance()
    {
        m_powerHauntDistance += Player.Instance.GetPlayerIncreaseHauntDistancePerUpgrade();
        DataPersistantManager.Instance.SaveGame();
        OnIncreaseHauntableDistance?.Invoke(this, EventArgs.Empty);
    }

    public int GetHauntableDistance()
    {
        return m_powerHauntDistance;
    }

}
