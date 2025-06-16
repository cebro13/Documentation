using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveStateRefSO", menuName = "Data/StateData/MoveData")]
public class MoveStateRefSO : ScriptableObject
{
    public float movementSpeed = 6f;
    public float acceleration = 20f;
    public float decceleration = 20f;
    public TextWritterLinesRefSO textWritterLinesRefSO;
}
