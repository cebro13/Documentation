using System;
using UnityEngine;

public class DetectColorBarrel : MonoBehaviour
{
    public event EventHandler<EventArgs> OnColorBarrelEnter;
    public event EventHandler<EventArgs> OnColorBarrelExit;

    [SerializeField] private bool m_canMoveBarrelAfter; //TODO NB:
    [SerializeField] private bool m_isColorActivate;

    [ShowIf("m_isColorActivate")]
    [SerializeField] private CustomColor.colorIndex m_colorIndex;

    private bool m_hasEnterAndWasGrab;

    private void Awake()
    {
        m_hasEnterAndWasGrab = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out BarrelLiquidColor barrelLiquidColor))
        {
            if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.PushState)
            {
                Debug.Log("Player.Instance.playerStateMachine.CurrentState == Player.Instance.PushState");
                Debug.Log("barrelLiquidColor " + barrelLiquidColor.name);
                Player.Instance.PushState.SetAbilityDone(true);
            }
            if(!m_isColorActivate)
            {
                barrelLiquidColor.MoveBarrelToAnchor(transform);
                OnColorBarrelEnter?.Invoke(this, EventArgs.Empty);
                return;
            }

            if(barrelLiquidColor.GetWaterColor() == m_colorIndex)
            {
                if(barrelLiquidColor.IsGrab())
                {
                    m_hasEnterAndWasGrab = true;
                    return;
                }
                barrelLiquidColor.MoveBarrelToAnchor(transform);
                OnColorBarrelEnter?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(m_hasEnterAndWasGrab)
        {
            if(collider.TryGetComponent(out BarrelLiquidColor barrelLiquidColor))
            {
                if(barrelLiquidColor.IsGrab())
                {
                    return;
                }
                else
                {
                    barrelLiquidColor.MoveBarrelToAnchor(transform);
                    OnColorBarrelEnter?.Invoke(this, EventArgs.Empty);
                    m_hasEnterAndWasGrab = false;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.TryGetComponent(out BarrelLiquidColor barrelLiquidColor))
        {
            if(!m_isColorActivate)
            {
                OnColorBarrelExit?.Invoke(this, EventArgs.Empty);
                return;
            }

            if(barrelLiquidColor.GetWaterColor() == m_colorIndex)
            {
                OnColorBarrelExit?.Invoke(this, EventArgs.Empty);
            }
        }
    }   
}
