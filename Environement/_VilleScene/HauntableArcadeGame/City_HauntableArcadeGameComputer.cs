using UnityEngine;
using System;

public class City_HauntableArcadeGameComputer : HauntableObject, IDataPersistant
{
    [SerializeField] private City_HauntableArcadeGameComputerScreenUI m_hauntableArcadeGameComputerScreenUI;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = Guid.NewGuid().ToString();
    }

    protected override void Awake()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        base.Awake();
    }

    public void LoadData(GameData data)
    {
        bool isCanHaunt = true;
        data.newDataPersistant.TryGetValue(m_ID, out isCanHaunt);
        if(!isCanHaunt)
        {
            NoLongerHauntable();
        }
    }

    public void SaveData(GameData data)
    {
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, IsCanHaunt());
    }

    protected override void Start()
    {
        base.Start();
        m_hauntableArcadeGameComputerScreenUI.OnGamerWin += ArcadeGame_OnGamerWin;
    }

    private void ArcadeGame_OnGamerWin(object sender, EventArgs e)
    {
        NoLongerHauntable();
        DataPersistantManager.Instance.SaveGame();
    }

    protected override void Update()
    {
        base.Update();
        if(m_isToProcessUpdate)
        {
                    
        }
    }

    public override void PlayerHauntStart()
    {
        base.PlayerHauntStart();
        Player.Instance.HauntingState.SetCanUnhaunt(false);
        m_hauntableArcadeGameComputerScreenUI.OpenComputer(ComputerOpenner.eComputerState.COMPUTER_STATE_INTERACTABLE);
    }

}
