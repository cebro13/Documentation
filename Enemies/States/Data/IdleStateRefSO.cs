using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IdleStateRefSO", menuName = "Data/StateData/IdleData")]
public class IdleStateRefSO : ScriptableObject
{
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;
}
