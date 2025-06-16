using UnityEngine;

[CreateAssetMenu(fileName = "SuicideAttackRefSO", menuName = "Data/StateData/SuicideAttackData")]
public class SuicideAttackStateRefSO : ScriptableObject
{
    public float chargeTime = 0.2f;
    public float attackRadius = 5f;
    public int damage = 1;
    public float stunKnockbackForce = 15f;
    public Vector2 stunKnockbackAngle;
    public GameObject suicideAttackParticulePrefab;
}