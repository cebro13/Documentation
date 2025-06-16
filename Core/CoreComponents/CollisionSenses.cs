using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class CollisionSenses : CoreComponent
{ 
    [SerializeField] private LayerMask m_groundLayerMask;
    [SerializeField] private LayerMask m_stairsLayerMask;
    [SerializeField] private Transform m_ledgeCheckTransform;
    [SerializeField] private Transform m_wallCheckTransform;
    [SerializeField] private Transform m_groundCheckTransform;
    [SerializeField] private Transform m_closeRangeActionCheckTransform;
    [SerializeField] private float m_wallCheckDistance = 0.4f;
    [SerializeField] private float m_ledgeCheckDistance = 0.2f;
    [SerializeField] private float m_groundCheckSize = 1f;
    [SerializeField] private float m_floatHeightGroundCheck = 2f;
    [SerializeField] private float m_minAggroDistance = 3f;
    [SerializeField] private float m_maxAggroDistance = 4f;
    [SerializeField] private float m_closeRangeActionDistance = 1f;

    //TODO Check if GetComponent() is better in the long run instead
    [SerializeField] private Collider2D m_colliderToFindPlayer;
    [SerializeField] private Transform m_centerTransform;

    protected Movement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<Movement>();
    }
    private Movement movement;

    private bool m_hasFoundPlayer;
    private Rigidbody2D m_rb;
    private Collider2D m_collider;

    private bool m_isPassingThroughPlatformDone;
    
    protected override void Awake()
    {
        base.Awake();
        m_hasFoundPlayer = false;
        m_isPassingThroughPlatformDone = true;
        m_rb = GetComponentInParent<Rigidbody2D>();
        m_collider = GetComponentInParent<Collider2D>();
    }

    public bool IsGrounded(bool isFloating = false)
    {
        if(!isFloating)
        {
            return Physics2D.OverlapBox(m_groundCheckTransform.position, new Vector2(m_groundCheckSize, 0.1f), 0, 1 << Player.GROUND_LAYER | 1 << Player.TWO_WAY_PLATFORM_LAYER);
        }
        else
        {
            Vector2 groundCheckPosition = m_groundCheckTransform.position;
            groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
            return Physics2D.OverlapBox(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck), 0, 1 << Player.GROUND_LAYER | 1 << Player.TWO_WAY_PLATFORM_LAYER);
        }
    }

    public RaycastHit2D FloatCastCollider(int layerMask)
    {
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
        return Physics2D.BoxCast(groundCheckPosition, new Vector2(m_groundCheckSize, 0.1f), 0f, Vector2.down, Player.Instance.GetPlayerFloatHeight(), layerMask); //-0.1f, to have transition between boxCast and capsule m_collider
    }

    public bool IsOnTwoWayPlatform(bool isFloating = false)
    {
        if(!isFloating)
        {
            return Physics2D.OverlapBox(m_groundCheckTransform.position, new Vector2(m_groundCheckSize, 0.1f), 0, 1 << Player.TWO_WAY_PLATFORM_LAYER);
        }
        else
        {
            Vector2 groundCheckPosition = m_groundCheckTransform.position;
            groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
            return Physics2D.OverlapBox(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck), 0, 1 << Player.TWO_WAY_PLATFORM_LAYER);
        }
    }

    public bool GetIsOnStair()
    {
        RaycastHit2D raycast = Physics2D.Raycast(m_ledgeCheckTransform.position, Vector2.down, m_ledgeCheckDistance, m_stairsLayerMask);
        if(raycast.collider != null)
        {
            if(raycast.collider.gameObject.GetComponent<IStair>() != null)
            {
                Player.Instance.ClimbStairsState.SetCurrentStairs(raycast.collider.gameObject);
                return true;
            }
        }
        return false;
    }

    public bool IsOnStair(bool isFloating = false)
    {
        Collider2D collider;
        if(!isFloating)
        {
            collider = Physics2D.OverlapBox(m_groundCheckTransform.position, new Vector2(m_groundCheckSize, 0.1f), 0, m_stairsLayerMask);
        }
        else
        {
            Vector2 groundCheckPosition = m_groundCheckTransform.position;
            groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
            collider = Physics2D.OverlapBox(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck/2), 0, m_stairsLayerMask);
        }
        if(collider != null)
        {
            Player.Instance.ClimbStairsState.SetCurrentStairs(collider.gameObject);
        }
        return collider != null;
    }

    public void IgnoreCollisionWith(Collider2D collider, bool isActive)
    {
        Physics2D.IgnoreCollision(collider, m_collider, isActive);
    }

    public void PassThroughOneWayPlatformUnder()
    {
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
        Collider2D collider = Physics2D.OverlapBox(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck), 0, 1 << Player.TWO_WAY_PLATFORM_LAYER);
        if(collider.GetComponent<PlatformEffector2D>() != null)
        {
            float delayToPassThrough = 1f;
            StartCoroutine(PassThroughPlatformFor(delayToPassThrough, collider));
        }
    }

    public bool IsSomethingAboveDash(float distanceToGround)
    {
        if(Physics2D.Raycast(new Vector2(m_collider.bounds.center.x + m_movement.GetFacingDirection() * m_collider.bounds.extents.x , m_collider.bounds.center.y), Vector2.up, distanceToGround, m_groundLayerMask).collider != null)
        {
            if(Physics2D.Raycast(new Vector2(m_collider.bounds.center.x + m_movement.GetFacingDirection() * m_collider.bounds.extents.x , m_collider.bounds.center.y), Vector2.down, distanceToGround, m_groundLayerMask).collider != null)
            {
                return true;
            }
        }
        else if(Physics2D.Raycast(new Vector2(m_collider.bounds.center.x, m_collider.bounds.center.y), Vector2.up, distanceToGround, m_groundLayerMask).collider != null)
        {
            if(Physics2D.Raycast(new Vector2(m_collider.bounds.center.x, m_collider.bounds.center.y), Vector2.down, distanceToGround, m_groundLayerMask).collider != null)
            {
                return true;
            }
        }
        else if(Physics2D.Raycast(new Vector2(m_collider.bounds.center.x - m_movement.GetFacingDirection() * m_collider.bounds.extents.x , m_collider.bounds.center.y), Vector2.up, distanceToGround, m_groundLayerMask).collider != null)
        {
            if(Physics2D.Raycast(new Vector2(m_collider.bounds.center.x - m_movement.GetFacingDirection() * m_collider.bounds.extents.x , m_collider.bounds.center.y), Vector2.down, distanceToGround, m_groundLayerMask).collider != null)
            {
                return true;
            }
        }
        return false;
    }

    public bool GetIsPassingThroughPlatformDone()
    {
        return m_isPassingThroughPlatformDone;
    }

    public bool IsGroundedOnLedge()
    {
        return Physics2D.Raycast(m_ledgeCheckTransform.position, Vector2.down, m_ledgeCheckDistance, 1 << Player.GROUND_LAYER | 1 << Player.TWO_WAY_PLATFORM_LAYER).collider != null;
    }

    public Transform GetLedgeCheckTransform()
    {
        return m_ledgeCheckTransform;
    }

    public Collider2D GetCollider2D()
    {
        return m_collider;
    }

    public bool CheckWall()
    {
        return Physics2D.Raycast(m_wallCheckTransform.position, m_rb.transform.right, m_wallCheckDistance, m_groundLayerMask); 
    }

    public bool CheckLedge()
    {
        return Physics2D.Raycast(m_ledgeCheckTransform.position, Vector2.down, m_ledgeCheckDistance, m_groundLayerMask);
    }

    public void CheckForPlayer()
    {
        if(!m_hasFoundPlayer)
        {
            if(m_colliderToFindPlayer.IsTouchingLayers(Player.Instance.GetPlayerLayerMask()))
            {
                Vector2 playerPosition = Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition();
                Vector2 dirToTarget = (playerPosition - (Vector2)m_centerTransform.position).normalized;
                float dstToTarget = Vector2.Distance(m_centerTransform.position, playerPosition);
                if(!Physics2D.Raycast(m_centerTransform.position, dirToTarget, dstToTarget, m_groundLayerMask))
                {
                    m_hasFoundPlayer = true;
                }
            }
            else
            {
                m_hasFoundPlayer = false;
            }
        }
        else if(m_hasFoundPlayer)
        {
            m_hasFoundPlayer = m_colliderToFindPlayer.IsTouchingLayers(Player.Instance.GetPlayerLayerMask());
        }
    }

    public bool CheckIfPlayerInFront()
    {
        if(m_movement.GetFacingDirection() == 1)
        {
            return Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_rb.transform.position.x > 0;
        }
        else
        {
            return Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_rb.transform.position.x < 0;
        }
    }

    public bool CheckPlayerInMinAggroRange()
    {
        if(m_hasFoundPlayer)
        {
            return Mathf.Abs(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_rb.transform.position.x) < m_minAggroDistance;
        }
        return false;
    }

    public bool CheckPlayerInMaxAggroRange()
    {
        if(m_hasFoundPlayer)
        {
            return Mathf.Abs(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition().x - m_rb.transform.position.x) < m_maxAggroDistance;
        }
        return false;
    }

    public bool CheckPlayerInCloseRangeAction()
    {
        if(m_hasFoundPlayer)
        {   
            return Mathf.Abs(Vector2.Distance(Player.Instance.Core.GetCoreComponent<PlayerMovement>().GetPosition(), m_closeRangeActionCheckTransform.position)) < m_closeRangeActionDistance;
        }
        return false;
    }

    private IEnumerator PassThroughPlatformFor(float delay, Collider2D collider)
    {
        Physics2D.IgnoreCollision(m_collider, collider, true);
        m_isPassingThroughPlatformDone = false;
        yield return new WaitForSeconds(delay);
        Physics2D.IgnoreCollision(m_collider, collider, false);
        m_isPassingThroughPlatformDone = true;
    }
    
    private void OnDrawGizmos()
    {
        //GroundCheck
        Gizmos.DrawWireCube(m_groundCheckTransform.position, new Vector3(m_groundCheckSize, 0.1f, 0));
        //LedgeCheck
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(m_ledgeCheckTransform.position, m_ledgeCheckTransform.position + (Vector3)(Vector2.down * m_ledgeCheckDistance));
        //WallCheck
        Gizmos.color = Color.white;
        Gizmos.DrawLine(m_wallCheckTransform.position, m_wallCheckTransform.position + (Vector3)(Vector2.right * m_wallCheckDistance));
        //GroundCheck float
        Gizmos.color = Color.red;
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
        Gizmos.DrawWireCube(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck/2));
        //CloseRangeAction
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_closeRangeActionCheckTransform.position, m_closeRangeActionDistance);
        //MaxAggroRange
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(m_centerTransform.position, m_centerTransform.position + (Vector3)(Vector2.right * m_maxAggroDistance));
        //MinAggroRange
        Gizmos.color = Color.red;
        Gizmos.DrawLine(m_centerTransform.position, m_centerTransform.position + (Vector3)(Vector2.right * m_minAggroDistance));
        
    }

}
