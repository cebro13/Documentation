using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PasswordMultipleInput : MonoBehaviour, IDataPersistant
{

    public event EventHandler<EventArgs> OnPasswordCorrect;
    [SerializeField] private List<GameObject> m_passwordClueList;
    [SerializeField] private string m_ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        m_ID = System.Guid.NewGuid().ToString();
    }

    private bool m_isFirstTimeSwitch;

    void Start()
    {
        if (string.IsNullOrEmpty(m_ID))
        {
            Debug.LogError("There needs to be an ID on every datapersistant object");
        }
        m_isFirstTimeSwitch = true;
        foreach(GameObject passwordClue in m_passwordClueList)
        {
            passwordClue.GetComponent<SpriteRenderer>().sprite = passwordClue.GetComponent<SwitchablePasswordClueList>().GetCurrentPasswordClueSO().sprite;
            passwordClue.GetComponent<SwitchablePasswordClueList>().OnSwitchPassword += SwitchablePasswordClueList_OnSwitchPassword;
        }
    }

    private void SwitchablePasswordClueList_OnSwitchPassword(object sender, EventArgs e)
    {
        bool isPasswordCorrect = true;
        foreach(GameObject passwordClue in m_passwordClueList)
        {
            passwordClue.GetComponent<SpriteRenderer>().sprite = passwordClue.GetComponent<SwitchablePasswordClueList>().GetCurrentPasswordClueSO().sprite;
            if(passwordClue.GetComponent<SwitchablePasswordClueList>().GetCurrentPasswordClueSO().ID != passwordClue.GetComponent<SwitchablePasswordClueList>().GetPasswordClueSOAnswer().ID)
            {
                isPasswordCorrect = false;
            }
        }
        if(isPasswordCorrect && m_isFirstTimeSwitch)
        {
            OnPasswordCorrect?.Invoke(this, EventArgs.Empty);
            m_isFirstTimeSwitch = false;
        }
    }

    public void LoadData(GameData data)
    {
        //This must be done because the out value in random if it's the first time;
        bool isFirstTimeSwitch = true;
        bool isThereData = data.passwordMultipleInputs.TryGetValue(m_ID, out m_isFirstTimeSwitch);
        if(!isThereData)
        {
            m_isFirstTimeSwitch = isFirstTimeSwitch;
        }
    }

    public void SaveData(GameData data)
    {
        if(data.passwordMultipleInputs.ContainsKey(m_ID))
        {
            data.passwordMultipleInputs.Remove(m_ID);
        }
        data.passwordMultipleInputs.Add(m_ID, m_isFirstTimeSwitch);
    }

}
