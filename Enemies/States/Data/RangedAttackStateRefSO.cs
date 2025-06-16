using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttackStateRefSO", menuName = "Data/StateData/RangedAttackData")]
public class RangedAttackStateRefSO : ScriptableObject
{
    public GameObject projectile;
    public float projectileStartAngle = 90f;
    public int projectileDamage = 3;
    public float projectileSpeed = 12f;
    public float timeAllowedToLive = 5f;
    public float knockbackForce = 2f;
    public Vector2 knockbackAngle = new Vector2(2f, 2f);
}
