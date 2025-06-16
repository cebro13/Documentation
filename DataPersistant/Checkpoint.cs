using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cinemachine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    public static event EventHandler<OnCheckPointTriggeredEventArgs> OnCheckpointTriggered;
    public class OnCheckPointTriggeredEventArgs : EventArgs
    {
        public int checkpoint;
    }

    [SerializeField] private int m_checkpointNumber;

    public static void ResetStaticData()
    {
        OnCheckpointTriggered = null;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER && DataPersistantManager.Instance.GetCheckpoint((Loader.Scene)SceneManager.GetActiveScene().buildIndex) != m_checkpointNumber)
        {
            OnCheckpointTriggered?.Invoke(this, new OnCheckPointTriggeredEventArgs{checkpoint = m_checkpointNumber});
        }
    }

    public int GetCheckpointNumber()
    {
        return m_checkpointNumber;
    }
}
