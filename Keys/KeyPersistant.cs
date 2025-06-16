using UnityEngine;
using System;

public class KeyPersistant : MonoBehaviour, IDataPersistant, ISwitchable, ICanInteract
{
    public static event EventHandler OnKeyCollected;

    public static void ResetStaticData()
    {
        OnKeyCollected = null;
    }

    [SerializeField] private ItemUIRefSO m_itemUIRefSO;
    [SerializeField] private string m_textToShowForUI;
    [SerializeField] private Sprite m_spriteToShowForUI;
    [SerializeField] private Utils.TriggerType m_triggerType;
    [SerializeField] private string m_ID;
    [SerializeField] private bool m_saveGameAfterGetKey = false;

    private bool m_isCollected = false;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    public void LoadData(GameData data)
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        data.keys.TryGetValue(m_ID, out m_isCollected);
        if(m_isCollected)
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(GameData data)
    {
        if(data.keys.ContainsKey(m_ID))
        {
            data.keys.Remove(m_ID);
        }
        data.keys.Add(m_ID, m_isCollected);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(m_triggerType != Utils.TriggerType.ColliderEnter)
        {
            return;
        }
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            HandleGetKey();
        }
    }

    public void Switch()
    {
        if(m_triggerType != Utils.TriggerType.Switch)
        {
            return;
        }
        HandleGetKey();
    }

    public void Interact()
    {
        if(m_triggerType != Utils.TriggerType.Interact)
        {
            return;
        }
        HandleGetKey();
    }

    public void NewItemFound()
    {
        Player.Instance.NewItemState.SetNewItemFound(m_itemUIRefSO.itemUIID, m_itemUIRefSO.ItemImage, m_textToShowForUI, PlayerDataManager.Instance.m_isFirstTimeItemFound);
        Player.Instance.playerStateMachine.ChangeState(Player.Instance.NewItemState);
    }

    private void HandleGetKey()
    {
        NewItemFound();
        OnKeyCollected?.Invoke(this, EventArgs.Empty);
        m_isCollected = true;
        if(m_saveGameAfterGetKey)
        {
            DataPersistantManager.Instance.SaveGame();
        }
        Destroy(gameObject);
    }
}
