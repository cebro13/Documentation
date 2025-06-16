using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Stair : MonoBehaviour, IStair
{
    [SerializeField] private Transform m_leftEdgeTransform;
    [SerializeField] private Transform m_rightEdgeTransform;
    [SerializeField] private float m_gravityScale = 4.5f;
    [SerializeField] private float m_maxVelocity = 7f;
    [SerializeField] private Utils.Direction m_facingDirection = Utils.Direction.Right;
    private Collider2D m_collider;
    
    private bool m_isActive;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Player.Instance.ClimbStairsState.OnActivateStairs += Player_OnActivateStairs;
        Player.Instance.ClimbStairsState.OnDeactivateStairs += Player_OnDeactivateStairs;
        Deactivate();
    }

    private void Player_OnActivateStairs(object sender, System.EventArgs e)
    {
        Activate();
    }

    private void Player_OnDeactivateStairs(object sender, System.EventArgs e)
    {
        Deactivate();
    }

    private void Update()
    {
        if(m_isActive) //If we leave the stair while another state is active.
        {
            if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.ClimbStairsState)
            {
                return;
            }
            if(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x > m_rightEdgeTransform.position.x)
            {
                Deactivate();
            }
            if(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x < m_leftEdgeTransform.position.x)
            {
                Deactivate();
            }
        }
    }

    public void Activate()
    {
        m_collider.isTrigger = false;
        m_isActive = true;
        Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetGravityScale(m_gravityScale);
        Player.Instance.ClimbStairsState.SetIsStairActive(true);
        Player.Instance.ClimbStairsState.SetMaxVelocityY(m_maxVelocity);
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.GROUND_LAYER, true);
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.TWO_WAY_PLATFORM_LAYER, true);  
    } 

    public void Deactivate()
    {
        m_collider.isTrigger = true;
        m_isActive = false;
        Player.Instance.Core.GetCoreComponent<PlayerMovement>().SetGravityScale(1f);
        Player.Instance.ClimbStairsState.SetIsStairActive(false);
        Player.Instance.ClimbStairsState.SetMaxVelocityY(0f);
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.GROUND_LAYER, false);   
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.TWO_WAY_PLATFORM_LAYER, false);  
    }

    public Transform GetRightTransform()
    {
        return m_rightEdgeTransform;
    }

    public Transform GetLeftTransform()
    {
        return m_leftEdgeTransform;
    }

    public Utils.Direction GetFacingDireciton()
    {
        return m_facingDirection;
    }
}
