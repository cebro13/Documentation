using UnityEngine;

[CreateAssetMenu(fileName = "FearReceipeRefSO", menuName = "Data/Fear/FearReceipeRefSO")]
public class FearReceipeRefSO : ScriptableObject
{
    public FearComponentRefSO fearComponentTopRefSO;
    public FearComponentRefSO fearComponentMiddleRefSO;
    public FearComponentRefSO fearComponentBottomRefSO;
    public GameObject fearMaterialization;
    public string fearName;
}
