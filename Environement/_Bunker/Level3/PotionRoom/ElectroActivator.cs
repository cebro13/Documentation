using System.Collections;
using UnityEngine;
using System;

public class ElectroActivator : MonoBehaviour, ISwitchable
{
    public event EventHandler<EventArgs> OnElectroActivatorActivate;
    public event EventHandler<EventArgs> OnElectroActivatorSwitch;

    [SerializeField] private LayerMask m_targetLayer;
    [SerializeField] private float m_overlapRadius = 0.5f;
    [SerializeField] private ParticleSystem m_particuleSys;
    [SerializeField] private Transform m_barrelPosition;
    [SerializeField] private float m_timerBetweenSwitch = 1.5f;

    private float m_timer;

    private void Awake()
    {
        m_timer = -5f;
    }

    public void Switch()
    {
        if(Time.time < m_timer + m_timerBetweenSwitch)
        {
            return;
        }
        m_timer = Time.time;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_barrelPosition.position, m_overlapRadius, m_targetLayer);
        m_particuleSys.Play();
        OnElectroActivatorSwitch?.Invoke(this, EventArgs.Empty);
        OnElectroActivatorActivate?.Invoke(this, EventArgs.Empty);
        foreach(Collider2D collider in colliders)
        {
            if(collider.TryGetComponent(out BarrelLiquidSwitchableElectricity barrelLiquidSwitchableElectricity))
            {
                StartCoroutine(CenterBarrelToLift(barrelLiquidSwitchableElectricity));
            }
        }
    }

    private IEnumerator CenterBarrelToLift(BarrelLiquidSwitchableElectricity barrelLiquidSwitchableElectricity)
    {
        while(Mathf.Abs(barrelLiquidSwitchableElectricity.transform.position.x - m_barrelPosition.position.x) > 0.1f )
        {
            barrelLiquidSwitchableElectricity.transform.position = Utils.MoveTowardsInX(barrelLiquidSwitchableElectricity.transform.position, m_barrelPosition.position, Time.deltaTime*2);
            yield return null;
        }
        barrelLiquidSwitchableElectricity.transform.position = new Vector2(m_barrelPosition.position.x, barrelLiquidSwitchableElectricity.transform.position.y);
        barrelLiquidSwitchableElectricity.transform.SetParent(transform);
        barrelLiquidSwitchableElectricity.Switch();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_barrelPosition.position, m_overlapRadius);
    }
}
