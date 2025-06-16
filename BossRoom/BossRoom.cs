using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private TriggerEnterRoom m_triggerEnterRoom;
    [SerializeField] private TriggerLeaveRoom m_trrigerLeaveRoom;

    private void Start()
    {
        m_triggerEnterRoom.OnEnterRoom += TriggerEnterRoom_OnEnterRoom;
        m_trrigerLeaveRoom.OnLeaveRoom += TriggerLeaveRoom_OnLeaveRoom;
    }

    private void TriggerEnterRoom_OnEnterRoom(object sender, EventArgs e)
    {
        
    }

    private void TriggerLeaveRoom_OnLeaveRoom(object sender, EventArgs e)
    {
        
    }
    
}
