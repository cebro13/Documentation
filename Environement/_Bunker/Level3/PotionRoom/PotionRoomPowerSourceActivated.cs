using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionRoomPowerSourceActivated : MonoBehaviour, ISwitchable
{
    [SerializeField] private PotionRoomDoor m_switchableDoorScript;

    public void Switch()
    {
        m_switchableDoorScript.HandlePotionRoomDoorActive(true);
    }
}
