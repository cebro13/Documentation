using UnityEngine;

[CreateAssetMenu(fileName = "StunStateRefSO", menuName = "Data/StateData/StunData")]
public class StunStateRefSO : ScriptableObject
{
    public float stunTime = 3f;
    public float groundDistance = 2f;
    public float stunKnockbackForce = 15f;
    public Vector2 stunKnockbackAngle;
}
