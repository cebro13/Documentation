using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        Loader.ResetStaticData();
        Checkpoint.ResetStaticData();
        SceneDataPersistant.ResetStaticData();
        KeyPersistant.ResetStaticData();
    }
}
