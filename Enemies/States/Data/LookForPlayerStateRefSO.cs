using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LookForPlayerStateRefSO", menuName = "Data/StateData/LookForPlayerData")]
public class LookForPlayerStateRefSO : ScriptableObject
{
    public int amountOfTurn = 2;
    public float timeBetweenTurns = 0.75f;
}
