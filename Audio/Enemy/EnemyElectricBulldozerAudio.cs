using FMOD.Studio;
using UnityEngine;
using FMODUnity;

public class EnemyElectricBulldozerAudio : MonoBehaviour
{
    [SerializeField] private EnemyElectricBulldozerAudioClipRefSO m_electricBulldozerAudioClipRefSO;
    [SerializeField] private EnemyElectricBulldozer m_enemyElectricBulldozer;
    
    private EventInstance m_audioBulldozerMove;
    private EventInstance m_audioBulldozerCharge;

    private void Start()
    {
        m_enemyElectricBulldozer.moveState.OnMoveStart += Bulldozer_OnMoveStart;
        m_enemyElectricBulldozer.moveState.OnMoveStop += Bulldozer_OnMoveStop;
        m_enemyElectricBulldozer.playerDetectedState.OnPlayerDetected += Bulldozer_OnPlayerDetected;
        m_enemyElectricBulldozer.chargeState.OnChargeStart += Bulldozer_OnChargeStart;
        m_enemyElectricBulldozer.chargeState.OnChargeStop += Bulldozer_OnChargeStop;
        m_enemyElectricBulldozer.meleeAttackState.OnMeleeAttackTrigger += Bulldozer_OnMeleeAttackTrigger;

        m_audioBulldozerMove = AudioManager.Instance.CreateInstance(m_electricBulldozerAudioClipRefSO.BulldozerMove);
        m_audioBulldozerCharge = AudioManager.Instance.CreateInstance(m_electricBulldozerAudioClipRefSO.BulldozerCharge);
        
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(Vector2.right),
            up = RuntimeUtils.ToFMODVector(Vector2.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero)
        };

        m_audioBulldozerMove.set3DAttributes(attributes);
        m_audioBulldozerMove.start();

        m_audioBulldozerCharge.set3DAttributes(attributes);
        m_audioBulldozerCharge.start();
    }

    private void Update()
    {
        FMOD.ATTRIBUTES_3D attributes = new FMOD.ATTRIBUTES_3D
        {
            position = RuntimeUtils.ToFMODVector(transform.position),
            forward = RuntimeUtils.ToFMODVector(transform.right),
            up = RuntimeUtils.ToFMODVector(transform.up),
            velocity = RuntimeUtils.ToFMODVector(Vector2.zero) 
        };

        // Update the attributes for all relevant FMOD instances
        m_audioBulldozerMove.set3DAttributes(attributes);
        m_audioBulldozerCharge.set3DAttributes(attributes);
    }

    private void Bulldozer_OnMoveStart(object sender, System.EventArgs e)
    {
       PLAYBACK_STATE playbackState;
       m_audioBulldozerMove.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioBulldozerMove.start();
       }
    }

    private void Bulldozer_OnMoveStop(object sender, System.EventArgs e)
    {
       m_audioBulldozerMove.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Bulldozer_OnChargeStart(object sender, System.EventArgs e)
    {
       PLAYBACK_STATE playbackState;
       m_audioBulldozerCharge.getPlaybackState(out playbackState);
       if(playbackState.Equals(PLAYBACK_STATE.STOPPED) || playbackState.Equals(PLAYBACK_STATE.STOPPING))
       {
            m_audioBulldozerCharge.start();
       }    
    }

    private void Bulldozer_OnChargeStop(object sender, System.EventArgs e)
    {
       m_audioBulldozerCharge.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Bulldozer_OnPlayerDetected(object sender, System.EventArgs e)
    {
        AudioManager.Instance.PlayOneShot(m_electricBulldozerAudioClipRefSO.BulldozerPlayerDetected, this.transform.position);
    }

    private void Bulldozer_OnMeleeAttackTrigger(object sender, System.EventArgs e)
    {
       AudioManager.Instance.PlayOneShot(m_electricBulldozerAudioClipRefSO.BulldozerMeleeAttack, this.transform.position);
    }
}
