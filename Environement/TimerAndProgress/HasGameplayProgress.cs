using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HasGameplayProgress : MonoBehaviour
{
    public event EventHandler<EventArgs> OnProgressFinished;
    public event EventHandler<EventArgs> OnProgressReset;
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;  
    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progress;
    }

    [SerializeField] private float m_progressStart;
    [SerializeField] private float m_progressFinish;
    [SerializeField] private bool m_goingUp = true;

    private float m_currentProgress;
    private bool m_isProgressFinished;

    private void Awake()
    {
        m_currentProgress = m_progressStart;
        m_isProgressFinished = false;
    }

    public void UpdateProgress(float progress)
    {
        if(m_isProgressFinished)
        {
            return;
        }

        if(m_goingUp)
        {
            m_currentProgress += progress;
            if(m_currentProgress >= m_progressFinish)
            {
                OnProgressFinished?.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        else
        {
            m_currentProgress -= progress;
            if(m_currentProgress <= m_progressFinish)
            {
                OnProgressFinished?.Invoke(this, EventArgs.Empty);
                return;                
            }
        }
        OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs{     
            progress = progress
        });
    }

    public void ResetProgress()
    {
        m_currentProgress = m_progressStart;
        OnProgressReset?.Invoke(this, EventArgs.Empty);
    }

    public float GetProgress()
    {
        return m_currentProgress;
    }
}
