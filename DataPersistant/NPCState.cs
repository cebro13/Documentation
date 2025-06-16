using System;

public interface INPCState<T> where T : Enum
{
    int GetState();
    void SetState(T state);
}

public enum eNPC
{
    #region GhostPNJ
    NPC_TANK_GHOST,
    NPC_FROZEN_GHOST,
    NPC_BLINKY,
    NPC_CLYDE,
    #endregion
    //
    #region HumanPNJ
    NPC_HARDCORE_GAMER,
    NPC_ELECTRONIC_STORE_SELLER
    #endregion
}

#region Ghost PNJ State
public enum eTankGhostState
{
    STATE_0_PRISONNED,
    STATE_1_PRISONNED_TALKED,
    STATE_2_RELEASED,
    STATE_3_NO_TALK_CITY,
    STATE_4_TALKED_CITY
}

public enum eFrozenGhostState
{
    STATE_0_PRISONNED,
    STATE_1_PRISONNED_TALKED,
    STATE_2_RELEASED,
    STATE_3_NO_TALK_CITY,
    STATE_4_TALKED_CITY
}

public enum eBlinkyState
{
    STATE_0_MINE,
    STATE_1_TALKED,
    STATE_2_CLYDE_SAVED_MINE,
    STATE_3_CLYDE_SAVED_NO_TALK_CITY,
    STATE_4_CLYDE_SAVED_TALKED_CITY,
    STATE_5_CLYDE_DEAD_NO_TALK_CITY,
    STATE_6_CLYDE_DEAD_TALKED_CITY
}

public enum eClydeState
{
    STATE_0_PRISONNED,
    STATE_1_PRISONNED_TALKED,
    STATE_2_RELEASED_NO_TALK,
    STATE_3_DEAD,
    STATE_4_NO_TALK_CITY,
    STATE_5_TALKED_CITY,
    STATE_6_OBJECT_FOUND_CITY
}

#endregion


#region Human PNJ State
public enum eHardcoreGamerState
{
    STATE_0_GAMING,
    STATE_1_GONE
}

public enum eElectronicStoreSellerState
{
    STATE_0_NO_TALK,
    STATE_1_NO_TALK_SUPRISED,
    STATE_2_TALKED,
    STATE_3_CAN_NOT_REPAIR_NO_TALK,
    STATE_4_CAN_NOT_REPAIR_TALKED,
    STATE_5_CAN_REPAIR_NO_TALK,
    STATE_6_CAN_REPAIR_TALKED
}
#endregion

[System.Serializable]
public abstract class NPCStateWrapper<T> : INPCState<T> where T : Enum
{
    public abstract T GetState();
    public abstract void SetState(T state);

    int INPCState<T>.GetState() => Convert.ToInt32(GetState()); // Maintain compatibility with integer-based systems}
}

[System.Serializable]
public class NPCStateTankGhostStateWrapper : NPCStateWrapper<eTankGhostState>
{
    public eTankGhostState State;

    public override eTankGhostState GetState() => State;

    public override void SetState(eTankGhostState state)
    {
        if (state is eTankGhostState npcState)
        {
            State = npcState;
        }
        else
        {
            throw new InvalidCastException($"Cannot assign {state.GetType()} to NPCStateTankGhostStateWrapper.");
        }
    }
}

[System.Serializable]
public class NPCStateFrozenGhostStateWrapper : NPCStateWrapper<eFrozenGhostState>
{
    public eFrozenGhostState State;

    public override eFrozenGhostState GetState() => State;

    public override void SetState(eFrozenGhostState state)
    {
        if (state is eFrozenGhostState npcState)
        {
            State = npcState;
        }
        else
        {
            throw new InvalidCastException($"Cannot assign {state.GetType()} to NPCStateFrozenGhostStateWrapper.");
        }
    }
}

[System.Serializable]
public class NPCStateBlinkyStateWrapper : NPCStateWrapper<eBlinkyState>
{
    public eBlinkyState State;

    public override eBlinkyState GetState() => State;

    public override void SetState(eBlinkyState state)
    {
        if (state is eBlinkyState npcState)
        {
            State = npcState;
        }
        else
        {
            throw new InvalidCastException($"Cannot assign {state.GetType()} to NPCStateBlinkyStateWrapper.");
        }
    }
}

[System.Serializable]
public class NPCStateClydeStateWrapper : NPCStateWrapper<eClydeState>
{
    public eClydeState State;

    public override eClydeState GetState() => State;

    public override void SetState(eClydeState state)
    {
        if (state is eClydeState npcState)
        {
            State = npcState;
        }
        else
        {
            throw new InvalidCastException($"Cannot assign {state.GetType()} to NPCStateClydeStateWrapper.");
        }
    }
}

[System.Serializable]
public class NPCStateHardcoreGamerWrapper : NPCStateWrapper<eHardcoreGamerState>
{
    public eHardcoreGamerState State;

    public override eHardcoreGamerState GetState() => State;

    public override void SetState(eHardcoreGamerState state)
    {
        if (state is eHardcoreGamerState npcState)
        {
            State = npcState;
        }
        else
        {
            throw new InvalidCastException($"Cannot assign {state.GetType()} to NPCStateHardcoreGamerWrapper.");
        }
    }
}

[System.Serializable]
public class NPCStateElectronicStoreSellerWrapper : NPCStateWrapper<eElectronicStoreSellerState>
{
    public eElectronicStoreSellerState State;

    public override eElectronicStoreSellerState GetState() => State;

    public override void SetState(eElectronicStoreSellerState state)
    {
        if (state is eElectronicStoreSellerState npcState)
        {
            State = npcState;
        }
        else
        {
            throw new InvalidCastException($"Cannot assign {state.GetType()} to NPCElectronicStoreSellerWrapper.");
        }
    }
}
