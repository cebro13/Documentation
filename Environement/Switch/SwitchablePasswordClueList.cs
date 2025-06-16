using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwitchablePasswordClueList : MonoBehaviour, ISwitchable
{
    public event EventHandler<EventArgs> OnSwitchPassword;

    [SerializeField] private PasswordClueListSO m_passwordClueListSO;
    [SerializeField] private PasswordClueSO m_passwordClueSOAnswer;

    private PasswordClueSO m_currentPasswordClueSO;
    private int m_indexPassword;

    private void Awake()
    {
        m_indexPassword = 3;
        m_currentPasswordClueSO = m_passwordClueListSO.passwordClueSOList[m_indexPassword];
    }

    public void Switch()
    {
        m_indexPassword += 1;
        if(m_indexPassword == m_passwordClueListSO.passwordClueSOList.Count)
        {
            m_indexPassword = 0;
        }
        m_currentPasswordClueSO = m_passwordClueListSO.passwordClueSOList[m_indexPassword];
        OnSwitchPassword?.Invoke(this, EventArgs.Empty);
    }

    public PasswordClueSO GetPasswordClueSOAnswer()
    {
        return m_passwordClueSOAnswer;
    }

    public PasswordClueSO GetCurrentPasswordClueSO()
    {
        return m_currentPasswordClueSO;
    }
}
