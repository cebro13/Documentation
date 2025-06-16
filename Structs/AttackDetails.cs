using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AttackDetails
{
    public Vector2 position;
    public int damageAmount;

    public float knockbackForce;
    public Vector2 knockbackAngle;
}
