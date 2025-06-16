using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChargeStateRefSO", menuName = "Data/StateData/ChargeData")]
public class ChargeStateRefSO : ScriptableObject
{
    public float chargeSpeed = 3f;
    public float chargeTime = 2f;
    public TextWritterLinesRefSO textWritterLinesRefSO;
}
