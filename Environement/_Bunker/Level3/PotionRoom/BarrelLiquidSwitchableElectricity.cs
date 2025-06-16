using UnityEngine;

public class BarrelLiquidSwitchableElectricity : MonoBehaviour, ISwitchable
{
    [SerializeField] private HasWaterColor m_hasChangeColor;
    [SerializeField] private WaterColorSettingsRefSO m_waterColorGreenRefSO;
    [SerializeField] private GameObject m_gameObjectElectricityBolt;
    [SerializeField] private SwitchableAudio m_switchableAudio;

    private bool m_isElectricityRunning;

    private void Awake()
    {
        m_isElectricityRunning = false;
    }

    public void Switch()
    {
        if(m_hasChangeColor.GetColorSettings().colorIndex == CustomColor.colorIndex.VIOLET)
        {
            m_isElectricityRunning = true;
            m_hasChangeColor.SetColor(m_waterColorGreenRefSO.colorSettings);
            m_gameObjectElectricityBolt.gameObject.SetActive(true);
            m_switchableAudio.Switch();
        }
    }

    /*public void Explode()
    {
        if(!m_isExplosionActive)
        {
            GameObject.Instantiate(m_explosionFX, transform.position, m_explosionFX.transform.rotation);
            CameraShakeManager.Instance.CameraExplosionFromProfile(m_screenShakeProfilerSmallRefSO, m_impulseSource);
            if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.PushState)
            {
                Player.Instance.PushState.BoxDestroyed();
            }
            Destroy(gameObject);
            return;
        }
        m_initialAttackDetails.position = transform.position;
        GameObject.Instantiate(m_explosionFX, transform.position, m_explosionFX.transform.rotation);
        Collider2D damageHit = Physics2D.OverlapCircle(transform.position, m_explositionRadius, m_playerLayer);
        Collider2D[] groundHit = Physics2D.OverlapCircleAll(transform.position, m_explositionRadius, m_groundLayer);
        if(damageHit)
        {
            PlayerCombat combatCoreComponent = Player.Instance.Core.GetCoreComponent<PlayerCombat>();
            combatCoreComponent.Damage(m_initialAttackDetails.damageAmount);
            combatCoreComponent.Knockback(m_initialAttackDetails.knockbackAngle, m_initialAttackDetails.knockbackForce, (int)Mathf.Sign(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - transform.position.x));
        }
        foreach(Collider2D collider in groundHit)
        {
            if(collider.TryGetComponent(out IExplodable explodableObject))
            {
                explodableObject.Explode();
            }
        }
        CameraShakeManager.Instance.CameraExplosionFromProfile(m_screenShakeProfilerBigRefSO, m_impulseSource);
        if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.PushState)
        {
            Player.Instance.PushState.BoxDestroyed();
        }
        Destroy(gameObject);
    }*/

    public bool GetIsElectricityRunning()
    {
        return m_isElectricityRunning;
    }
}
