using System;
using UnityEngine;

public class PlayerFollowWaypointsState : PlayerAbilityState
{
    public event EventHandler<EventArgs> OnFollowWaypointsStart;
    public event EventHandler<EventArgs> OnFollowWaypointsCancel;

    private Waypoints m_waypoints;
    private float m_distanceThreshold = 0.1f;

    private Transform m_currentWaypoint;
    private Transform m_nextWaypoint;
    private bool m_isLastWayPoint;

    public PlayerFollowWaypointsState(PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : 
    base(playerStateMachine, playerData, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        OnFollowWaypointsStart?.Invoke(this, EventArgs.Empty);
        m_movement.SetRigidBodyStatic(true);
        m_waypoints.GetNextWaypoint(m_currentWaypoint, out m_nextWaypoint);
        m_isLastWayPoint = false;
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.GROUND_LAYER, true);
    }

    public override void Exit()
    {
        base.Exit();
        OnFollowWaypointsCancel?.Invoke(this, EventArgs.Empty);
        m_movement.SetRigidBodyStatic(false);
        Physics2D.IgnoreLayerCollision(Player.PLAYER_LAYER, Player.GROUND_LAYER, false);
        m_waypoints = null;
        m_currentWaypoint = null;
    }

    public override int LogicUpdate()
    {
        int retValue = base.LogicUpdate();
        if(retValue != 0) return retValue;
        if(!m_isLastWayPoint)
        {
            if(Vector2.Distance(m_movement.GetPosition(), m_currentWaypoint.position) < m_distanceThreshold && !m_isLastWayPoint)
            {
                m_isLastWayPoint = m_waypoints.GetNextWaypoint(m_currentWaypoint, out m_nextWaypoint);
                m_currentWaypoint = m_nextWaypoint;
            }
        }
        else
        {
            if(Vector2.Distance(m_movement.GetPosition(), m_currentWaypoint.position) < m_distanceThreshold)
            {
                m_isAbilityDone = true;
            }
        }
        //This is a set position, not a physics change. This belong in Update.
        m_movement.SetPosition(Vector2.MoveTowards(Player.Instance.transform.position, m_currentWaypoint.position, m_playerData.followWaypointsSpeed * Time.deltaTime));
        return 0;
    }

    public void SetWaypoints(Transform startWaypoint, Waypoints waypoints)
    {
        m_currentWaypoint = startWaypoint;
        m_waypoints = waypoints;
    }
}
