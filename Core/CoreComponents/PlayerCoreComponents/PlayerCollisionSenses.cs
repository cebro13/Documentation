using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class PlayerCollisionSenses : CoreComponent
{
    [SerializeField] private LayerMask m_groundLayerMask;
    [SerializeField] private LayerMask m_interactableLayerMask;
    [SerializeField] private LayerMask m_grabableLayerMask;
    [SerializeField] private LayerMask m_stairsLayerMask;
    [SerializeField] private LayerMask m_waterLayerMask;
    [SerializeField] private Transform m_ledgeCheckTransform;
    [SerializeField] private Transform m_groundCheckTransform;
    [SerializeField] private Transform m_grabCheckTransform;
    [SerializeField] private Transform m_waterCheckTransform;
    [SerializeField] private Transform m_dashEmptyCheckTransform;
    [SerializeField] private Transform m_crushCheckUpTransform;
    [SerializeField] private Transform m_crushCheckDownTransform;

    [SerializeField] private float m_ledgeCheckDistance = 0.2f;
    [SerializeField] private float m_groundCheckSize = 1f;
    [SerializeField] private float m_waterCheckSizeHeight = 2f;
    [SerializeField] private float m_waterCheckSizeWidth = 1f;
    [SerializeField] private float m_floatHeightGroundCheck = 2f;
    [SerializeField] private float m_interactionRadius = 2f;
    [SerializeField] private float m_grabRadius = 1f;
    [SerializeField] private float m_crushCheckRadius = 0.5f;
    [SerializeField] private float m_dashEmptyCheckDistance = 0.2f;

    protected PlayerMovement m_movement
    {
        get => movement ??= m_core.GetCoreComponent<PlayerMovement>();
    }
    private PlayerMovement movement;

    private Rigidbody2D m_rb;
    private Collider2D m_collider;

    private bool m_isPassingThroughPlatformDone;
    
    protected override void Awake()
    {
        base.Awake();
        m_isPassingThroughPlatformDone = true;
        m_rb = GetComponentInParent<Rigidbody2D>();
        m_collider = GetComponentInParent<Collider2D>();
    }

    public bool IsGrounded()
    {
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y += m_floatHeightGroundCheck / 2f;
        bool isGrounded = Physics2D.OverlapBox(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck), 0, 1 << Player.GROUND_LAYER | 1 << Player.TWO_WAY_PLATFORM_LAYER);
        return isGrounded;
    }

    public bool IsTouchingWater()
    {
        Vector2 waterCheckPosition = m_waterCheckTransform.position;
        return Physics2D.OverlapBox(waterCheckPosition, new Vector2(m_waterCheckSizeWidth, m_waterCheckSizeHeight), 0, 1 << Player.WATER_LAYER);
    }

    public RaycastHit2D FloatCastCollider(int layerMask)
    {
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
        return Physics2D.BoxCast(groundCheckPosition, new Vector2(m_groundCheckSize, 0.1f), 0f, Vector2.down, Player.Instance.GetPlayerFloatHeight(), layerMask); //-0.1f, to have transition between boxCast and capsule m_collider
    }

    public bool IsOnTwoWayPlatform()
    {
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
        return Physics2D.OverlapBox(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck), 0, 1 << Player.TWO_WAY_PLATFORM_LAYER);
        
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

    public bool IsOnStair()
    {
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y = m_groundCheckTransform.position.y + m_floatHeightGroundCheck/2;
        Collider2D collider = Physics2D.OverlapBox(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck/2), 0, m_stairsLayerMask);
        
        if(collider != null)
        {
            Player.Instance.ClimbStairsState.SetCurrentStairs(collider.gameObject);
        }
        return collider != null;
    }
    //TODO Remove gameObject?
    public bool CheckIfHasPlayerChangeState(out PlayerState playerState)
    {   
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_rb.position, m_interactionRadius, m_interactableLayerMask);
        foreach(Collider2D collider in colliders)
        {
            if(collider.TryGetComponent(out IHasPlayerChangeState hasPlayerChangeState))
            {
                playerState = hasPlayerChangeState.GetPlayerState();
                if(playerState != null)
                {
                    return true;
                }
            }
        }
        playerState = null;
        return false;
    }

    public bool CheckIfHasPlayerGrabChangeState(out PlayerState playerState)
        {   
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_rb.position, m_grabRadius, m_grabableLayerMask);
        foreach(Collider2D collider in colliders)
        {
            if(collider.TryGetComponent(out IHasPlayerChangeState hasPlayerChangeState))
            {
                playerState = hasPlayerChangeState.GetPlayerState();
                if(playerState != null)
                {
                    return true;
                }
            }
        }
        playerState = null;
        return false;
    }

    public bool CheckIfHasCanInteract(out List<ICanInteract> canInteractGameObject)
    {   
        canInteractGameObject = new List<ICanInteract>();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_rb.position, m_interactionRadius, m_interactableLayerMask);
        bool retValue = false;
        foreach(Collider2D collider in colliders)
        {
            var mObjs = collider.GetComponents<MonoBehaviour>();
            ICanInteract[] interfaceScripts = (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(ICanInteract)) select (ICanInteract)a).ToArray();
            if(interfaceScripts.Length == 0)
            {
                return retValue;
            }
            foreach(ICanInteract canInteract in interfaceScripts)
            {
                canInteractGameObject.Add(canInteract);
                retValue = true;
            }
        }
        return retValue;
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
        //return Physics2D.Raycast(m_ledgeCheckTransform.position, Vector2.down, m_ledgeCheckDistance, 1 << Player.GROUND_LAYER | 1 << Player.TWO_WAY_PLATFORM_LAYER).collider != null;
        return Physics2D.Raycast(m_ledgeCheckTransform.position, Vector2.down, m_ledgeCheckDistance, 1 << Player.GROUND_LAYER).collider != null;

    }

    public bool IsDashEmptyOnLedge()
    {
        return Physics2D.Raycast(m_dashEmptyCheckTransform.position, Vector2.down, m_dashEmptyCheckDistance, 1 << Player.GROUND_LAYER | 1 << Player.TWO_WAY_PLATFORM_LAYER).collider != null;
    }

    public Transform GetLedgeCheckTransform()
    {
        return m_ledgeCheckTransform;
    }

    public Collider2D GetCollider2D()
    {
        return m_collider;
    }

    public bool CheckLedge()
    {
        return Physics2D.Raycast(m_ledgeCheckTransform.position, Vector2.down, m_ledgeCheckDistance, m_groundLayerMask);
    }

    public bool CheckIfCrushed()
    {
        if(Physics2D.OverlapCircle(m_crushCheckUpTransform.position, m_crushCheckRadius, 1 << Player.GROUND_LAYER))
        {
            if(Physics2D.OverlapCircle(m_crushCheckDownTransform.position, m_crushCheckRadius, 1 << Player.GROUND_LAYER))
            {
                return true;
            }
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
        //GroundCheck float
        Gizmos.color = Color.red;
        Vector2 groundCheckPosition = m_groundCheckTransform.position;
        groundCheckPosition.y += m_floatHeightGroundCheck / 2f;
        Gizmos.DrawWireCube(groundCheckPosition, new Vector2(m_groundCheckSize, m_floatHeightGroundCheck));
        //GrabCheck
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(m_grabCheckTransform.position, m_grabRadius);
        //InteractCheck
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_grabCheckTransform.position, m_interactionRadius);
        //DashEmptyCheck
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(m_dashEmptyCheckTransform.position, m_dashEmptyCheckTransform.position + (Vector3)(Vector2.down * m_dashEmptyCheckDistance));
        //CrushCheck
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_crushCheckDownTransform.position, m_crushCheckRadius);
        Gizmos.DrawWireSphere(m_crushCheckUpTransform.position, m_crushCheckRadius);
    }

}
