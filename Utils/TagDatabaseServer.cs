using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagDatabaseServer : MonoBehaviour
{
    public const string SPRITE_1_TAG = "Sprite1";
    public const string GROUND_TAG = "Ground";
    public const string DETECTION_TAG = "Detection";
    public const string WATERFALL_TAG = "WaterFall";

    public static TagDatabaseServer Instance {get; private set;}

    [SerializeField] private SpawnOnTopOfPlayerRefSO m_spawnOnTopOfPlayerRefSo;
    private Dictionary<TagDatabaseId, GameObject> m_gameObjectTagDictionnary = new Dictionary<TagDatabaseId, GameObject>();

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        foreach(ObjectToSpawnPair objectToSpawnPair in m_spawnOnTopOfPlayerRefSo.TagStringDb)
        {
            m_gameObjectTagDictionnary.Add(objectToSpawnPair.id, objectToSpawnPair.gameObject);
        }
    }

    public GameObject GetGameObjectFromId(TagDatabaseId id)
    {
        GameObject gameObject = m_gameObjectTagDictionnary.GetValueOrDefault(id);
        if(gameObject == null)
        {
            Debug.LogError("Erreur: il n'y a pas de tagString associée à l'ID " + id);
        }
        return m_gameObjectTagDictionnary.GetValueOrDefault(id);
    } 
}
