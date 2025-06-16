using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistant
{
    void LoadData(GameData data);

    void SaveData(GameData data);
}
