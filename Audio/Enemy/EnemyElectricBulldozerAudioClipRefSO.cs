using FMODUnity;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyElectricBulldozerAudioClipRefSO", menuName = "Data/Audio/Enemy/ElectricBulldozerAudioClip")]
public class EnemyElectricBulldozerAudioClipRefSO : ScriptableObject
{
    public EventReference BulldozerMove;
    public EventReference BulldozerCharge;
    public EventReference BulldozerPlayerDetected;
    public EventReference BulldozerMeleeAttack;
}
