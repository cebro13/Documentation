using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnOnTopOfPlayerRefSO", menuName = "Data/Player/SpawnOnTopOfPlayer")]
public class SpawnOnTopOfPlayerRefSO : ScriptableObject
{
    public List<ObjectToSpawnPair> TagStringDb;
}

[Serializable]
public struct ObjectToSpawnPair
{
    public GameObject gameObject;
    public TagDatabaseId id;
    ObjectToSpawnPair(GameObject gameObject, TagDatabaseId id)
    {
        this.gameObject = gameObject;
        this.id = id;
    }
}
