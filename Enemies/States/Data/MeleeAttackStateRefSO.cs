using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "MeleeAttackStateRefSO", menuName = "Data/StateData/MeleeAttackData")]
public class MeleeAttackStateRefSO : ScriptableObject
{
    public float attackRadius = 0.5f;
    public int attackDamage = 2;
    public float knockbackForce = 5f;
    public Vector2 knockbackAngle = new Vector2 (0.5f, 0.5f);
}
