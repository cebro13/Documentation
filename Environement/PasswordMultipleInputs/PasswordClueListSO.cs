using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PasswordClueListSO", menuName = "Data/PasswordClueList")]
public class PasswordClueListSO : ScriptableObject
{
    public List<PasswordClueSO> passwordClueSOList;
}
