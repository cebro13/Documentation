using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CameraData", menuName = "Data/CameraData/BaseData")]
public class CameraData : ScriptableObject
{
    [Header("Base position Z")]
    public float maxMoveSpeed = -23.6f;
}
