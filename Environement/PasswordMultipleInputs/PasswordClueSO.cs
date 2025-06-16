using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PasswordClueSO", menuName = "Data/PasswordClue")]
public class PasswordClueSO : ScriptableObject
{
    public Sprite sprite;
    public string ID;

    [ContextMenu("Generate guid for ID")]
    private void GenerateGuid()
    {
        ID = System.Guid.NewGuid().ToString();
    }
}
