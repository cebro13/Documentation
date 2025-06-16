using System;
using UnityEngine;

public class ComputerConspirationiste : MonoBehaviour, ICanInteract, IDataPersistant
{
    [SerializeField] private ComputerConspirationisteUI m_computerConspirationistUI;
    [SerializeField] private string m_ID;

    [SerializeField] private bool m_isDataPersistantTest = true;

    private bool m_hasPasswordBeenFound = false;
    
    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_computerConspirationistUI.OnPasswordFound += ComputerConspirationUI_OnPasswordFound;
    }

    private void ComputerConspirationUI_OnPasswordFound(object sender, EventArgs e)
    {
        m_hasPasswordBeenFound = true;
        DataPersistantManager.Instance.SaveGame();
    }

    public void LoadData(GameData data)
    {
        data.newDataPersistant.TryGetValue(m_ID, out m_hasPasswordBeenFound);
    }

    public void SaveData(GameData data)
    {
        if(!m_isDataPersistantTest)
        {
            return;
        }
        if(data.newDataPersistant.ContainsKey(m_ID))
        {
            data.newDataPersistant.Remove(m_ID);
        }
        data.newDataPersistant.Add(m_ID, m_hasPasswordBeenFound);
    }

    public void Interact()
    {
        m_computerConspirationistUI.OpenComputer(m_hasPasswordBeenFound);
    }

}
