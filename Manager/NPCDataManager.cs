using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDataManager : MonoBehaviour, IDataPersistant
{
    public static NPCDataManager Instance {get; private set;}
    #region Ghost NPC
    public NPCStateTankGhostStateWrapper m_npcGilleState {get; private set;}
    public NPCStateFrozenGhostStateWrapper m_npcFrozenGhostState {get; private set;}
    public NPCStateBlinkyStateWrapper m_npcBlinkyState {get; private set;}
    public NPCStateClydeStateWrapper m_npcClydeState {get; private set;}
    #endregion

    #region  Human NPC
    public NPCStateHardcoreGamerWrapper m_npcHardcoreGamerState {get; private set;}
    public NPCStateElectronicStoreSellerWrapper m_npcElectronicStoreSellerState {get; private set;}
    #endregion

    private void Awake()
    {
        Instance = this;
        
        m_npcGilleState = new NPCStateTankGhostStateWrapper();
        m_npcFrozenGhostState = new NPCStateFrozenGhostStateWrapper();
        m_npcBlinkyState = new NPCStateBlinkyStateWrapper();
        m_npcClydeState = new NPCStateClydeStateWrapper();

        m_npcHardcoreGamerState = new NPCStateHardcoreGamerWrapper();
        m_npcElectronicStoreSellerState = new NPCStateElectronicStoreSellerWrapper();
    }
    
    public void LoadData(GameData data)
    {
        m_npcGilleState.SetState((eTankGhostState)data.npcGilleState);
        m_npcFrozenGhostState.SetState((eFrozenGhostState)data.npcFrozenGhostState);
        m_npcBlinkyState.SetState((eBlinkyState)data.npcBlinkyState);
        m_npcClydeState.SetState((eClydeState)data.npcClydeState);
        if(m_npcClydeState.GetState() == eClydeState.STATE_2_RELEASED_NO_TALK)
        {
            SetNewNPCState<eClydeState>(eNPC.NPC_CLYDE, eClydeState.STATE_4_NO_TALK_CITY);
        }

        m_npcHardcoreGamerState.SetState((eHardcoreGamerState)data.npcHardcoreGamerState);
        m_npcElectronicStoreSellerState.SetState((eElectronicStoreSellerState)data.npcElectronicSellerState);
    }

    public void SaveData(GameData data)
    {
        data.npcGilleState = (int)m_npcGilleState.GetState();
        data.npcFrozenGhostState = (int)m_npcFrozenGhostState.GetState();
        data.npcBlinkyState = (int)m_npcBlinkyState.GetState();
        data.npcClydeState = (int)m_npcClydeState.GetState();

        data.npcHardcoreGamerState = (int)m_npcHardcoreGamerState.GetState();
        data.npcElectronicSellerState = (int)m_npcElectronicStoreSellerState.GetState();
    }

    public void SetNewNPCState<T>(eNPC npc, T npcState, bool saveGame = true)
    {
        switch(npc)
        {
            case eNPC.NPC_TANK_GHOST:
            {
                if (npcState is eTankGhostState gilleState)
                {
                    m_npcGilleState.SetState(gilleState);
                }
                else
                {
                    Debug.LogError("Invalide enum chosen");
                }
                break;
            }
            case eNPC.NPC_FROZEN_GHOST:
            {
                if (npcState is eFrozenGhostState frozenGhostState)
                {
                    m_npcFrozenGhostState.SetState(frozenGhostState);
                }
                else
                {
                    Debug.LogError("Invalide enum chosen");
                }
                break;
            }
            case eNPC.NPC_BLINKY:
            {
                if (npcState is eBlinkyState blinkyState)
                {
                    m_npcBlinkyState.SetState(blinkyState);
                }
                else
                {
                    Debug.LogError("Invalide enum chosen");
                }
                break;
            }
            case eNPC.NPC_CLYDE:
            {
                if (npcState is eClydeState clydeState)
                {
                    m_npcClydeState.SetState(clydeState);
                }
                else
                {
                    Debug.LogError("Invalide enum chosen");
                }
                break;
            }
            case eNPC.NPC_HARDCORE_GAMER:
            {
                if (npcState is eHardcoreGamerState state)
                {
                    m_npcHardcoreGamerState.SetState(state);
                }
                else
                {
                    Debug.LogError("Invalide enum chosen");
                }
                break;
            }
            case eNPC.NPC_ELECTRONIC_STORE_SELLER:
            {
                if (npcState is eElectronicStoreSellerState state)
                {
                    m_npcElectronicStoreSellerState.SetState(state);
                }
                else
                {
                    Debug.LogError("Invalide enum chosen");
                }
                break;
            }
            default:
            {
                Debug.LogError("Invalide NPC chosen");
                break;
            }
        }
        if(saveGame)
        {
            DataPersistantManager.Instance.SaveGame();
        }
    }

    public T GetNPCState<T>(eNPC npc)
    {
        switch(npc)
        {
            case eNPC.NPC_TANK_GHOST:
            {
                return (T)(object)m_npcGilleState.GetState();
            }
            case eNPC.NPC_FROZEN_GHOST:
            {
                return (T)(object)m_npcFrozenGhostState.GetState();
            }
            case eNPC.NPC_BLINKY:
            {
                return (T)(object)m_npcBlinkyState.GetState();
            }
            case eNPC.NPC_CLYDE:
            {
                return (T)(object)m_npcClydeState.GetState();
            }
            case eNPC.NPC_HARDCORE_GAMER:
            {
                return (T)(object)m_npcHardcoreGamerState.GetState();
            }
            case eNPC.NPC_ELECTRONIC_STORE_SELLER:
            {
                return (T)(object)m_npcElectronicStoreSellerState.GetState();
            }
            default:
            {
                Debug.LogError("This case should not happens!");
                return default(T);
            }
        }
    }
}
