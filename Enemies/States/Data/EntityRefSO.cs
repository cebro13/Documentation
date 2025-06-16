using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityRefSO", menuName = "Data/EntityData/BaseData")]
public class EntityRefSO : ScriptableObject
{
    public int maxHealth = 10;
    public float damageHopSpeed = 3f;
    public float floatHeight = 1.5f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;

    public GameObject hitParticule;
}


