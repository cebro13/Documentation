using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchableSaveGame : MonoBehaviour, ISwitchable
{
    public void Switch()
    {
        DataPersistantManager.Instance.SaveGame();
    }
}
