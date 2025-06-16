using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    public void FireProjectile(float startAngle, float speed, float timeAllowedToLive, int damage, Vector2 knockbackAngle, float knockbackForce);
}
