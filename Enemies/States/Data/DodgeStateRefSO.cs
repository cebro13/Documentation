using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DodgeStateRefSO", menuName = "Data/StateData/DodgeData")]
public class DodgeStateRefSO : ScriptableObject
{
    public float dodgeForce = 20f;
    public float dodgeTime = 0.2f;
    public float dodgeCooldown = 2f;
    public Vector2 dodgeAngle;
}
