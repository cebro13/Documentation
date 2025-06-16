using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FearedStateRefSO", menuName = "Data/StateData/FearedData")]
public class FearedStateRefSO : ScriptableObject
{
    public int fearHealth = 2;
    public float fearHopForce = 2f;
    public GameObject fearSprite;
}
