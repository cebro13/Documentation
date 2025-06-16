using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System;

public class PushableGrabableBox : MonoBehaviour, IHasPlayerChangeState, IPushable, IGrabable
{
    public event EventHandler<EventArgs> OnBoxPushStart;
    public event EventHandler<EventArgs> OnBoxPushStop;

    [SerializeField] private float m_speed;
    [SerializeField] private List<GameObject> m_anchorPoints;
    [SerializeField] private float m_massWhenGrabbed = 3f;
    [SerializeField] private BoxCollider2D m_boxColGroundLeft;
    [SerializeField] private BoxCollider2D m_boxColGroundRight;
    [SerializeField] private BoxCollider2D m_boxColGroundNegative;
    [SerializeField] private LayerMask m_groundLayerMask;
    [SerializeField] private float m_speedToMoveBoxToAnchor = 16f;

    private bool m_playerInPushingRange;
    private bool m_hasBeenGrabbed;
    private bool m_isGrabbed;
    private List<Collider2D> m_anchorPointsCollider;
    private Rigidbody2D m_rb;
    private HasFriction m_hasFriction;
    private float m_initialMass;
    private bool m_isBoxMoving;
    private bool m_isPushed;
    private int m_direction;
    private bool m_isMovingBoxAtAnchor;
    private GameObject m_currentAnchorGameObject;
    private HandleGrabable m_currentHandleGrabable;
    private bool m_isBoxAtAnchor;
    private bool m_isMovingBoxAtTransform;
    private Transform m_currentTransformMove;

    public PlayerState GetPlayerState()
    {
        if(Player.Instance.Core.GetCoreComponent<PlayerCollisionSenses>().IsGrounded())
        {
            Initialize();
            return Player.Instance.PushState;
        }
        return null;
    }

    private void Initialize()
    {
        Player.Instance.PushState.SetPushable(this);
        Player.Instance.PushState.SetSpeed(m_speed);
        Player.Instance.PushState.SetTransformToFace(transform);
    }

    private void Awake()
    {
        m_hasBeenGrabbed = false;
        m_isMovingBoxAtTransform = false;
        m_rb = GetComponent<Rigidbody2D>();
        m_initialMass = m_rb.mass;
        m_hasFriction = GetComponent<HasFriction>();
        m_anchorPointsCollider = new List<Collider2D>();
        m_isBoxMoving = false;
        m_isPushed = false;
        m_isBoxAtAnchor = false;
        m_isMovingBoxAtAnchor = false;
        foreach(GameObject anchorPoint in m_anchorPoints)
        {
            Collider2D collider = anchorPoint.GetComponent<Collider2D>();
            if(collider == null)
            {
                Debug.LogError("GameObjet" + collider + " does not have a component that implements Collider2D");
            }
            m_anchorPointsCollider.Add(anchorPoint.GetComponent<Collider2D>());
        }
    }

    public bool GrabbedBy(HandleGrabable handleGrabable)
    {
        foreach(GameObject anchorGameObject in m_anchorPoints)
        {
            Collider2D anchorCollider = anchorGameObject.GetComponent<Collider2D>();
            Collider2D handleCollider = handleGrabable.GetComponent<Collider2D>();
            if(anchorCollider.IsTouching(handleCollider))
            {
                m_isMovingBoxAtAnchor = true;
                m_currentAnchorGameObject = anchorGameObject;
                m_currentTransformMove = anchorGameObject.transform;
                m_hasFriction.enabled = false;
                m_hasBeenGrabbed = true;
                m_currentHandleGrabable = handleGrabable;
                m_isBoxAtAnchor = false;
                return true;
            }
        }
        return false;
    }

    private void HandleGrab()
    {
        Anchor newAnchor;
        newAnchor.localPosition = m_currentTransformMove.localPosition;
        newAnchor.localRotation = m_currentTransformMove.rotation.eulerAngles.z;
        newAnchor.rigidbody = m_rb;
        m_rb.mass = m_massWhenGrabbed;
        m_isGrabbed = true;
        m_currentHandleGrabable.AnchorToThis(newAnchor);
    }

    public bool IsAtAnchor()
    {
        return m_isBoxAtAnchor;
    }

    public void MoveAtTransform(Transform transformTransform)
    {
        m_currentTransformMove = transformTransform;
        m_isBoxAtAnchor = false;
        m_isMovingBoxAtTransform = true;
    }

    public void ReleasedBy(HandleGrabable handleGrabable)
    {
        m_isGrabbed = false;
        m_currentAnchorGameObject = null;
        m_currentHandleGrabable = null;
        m_currentTransformMove = null;
    }

    public bool IsGrab()
    {
        return m_isGrabbed;
    }

    private void Update()
    {
        HandlePush();
        if(m_isGrabbed)
        {
            return;
        }

        if(m_rb.angularVelocity > 0.2f || m_rb.angularVelocity < -0.2f)
        {
            return;
        }

        if(m_isMovingBoxAtTransform)
        {
            Vector2 moveTowards = Vector2.MoveTowards(transform.position, m_currentTransformMove.position, m_speedToMoveBoxToAnchor * Time.deltaTime);
            transform.position = new Vector2(moveTowards.x, transform.position.y);
            if (Mathf.Abs(transform.position.x - m_currentTransformMove.position.x) < 0.1f)
            {
                m_isMovingBoxAtTransform = false;
                m_isBoxAtAnchor = true;
                m_currentTransformMove = null;
            }
        }

        if(m_isMovingBoxAtAnchor)
        {
            Vector2 moveTowards = Vector2.MoveTowards(transform.position, m_currentTransformMove.position, m_speedToMoveBoxToAnchor * Time.deltaTime);
            transform.position = new Vector2(moveTowards.x, transform.position.y);
            if (Mathf.Abs(transform.position.x - m_currentTransformMove.position.x) < 0.1f)
            {
                m_isMovingBoxAtAnchor = false;
                m_isBoxAtAnchor = true;
                HandleGrab();
            }
        }

        bool isRotationDone = false;
        float rotation = transform.rotation.z;

        while(rotation < 0)
        {
            rotation += 360f;
        }

        if((rotation < 1f && rotation > -1f) || (rotation < 361 && rotation > 359f))
        {
            m_rb.angularVelocity = 0f;
            transform.rotation = Quaternion.Euler(0,0,0);
            isRotationDone = true;
        }
        else if(rotation < 91f && rotation > 89f)
        {
            m_rb.angularVelocity = 0f;
            transform.rotation = Quaternion.Euler(0,0,90);
            isRotationDone = true;
        }
        else if(rotation < 181f && rotation > 179f)
        {
            m_rb.angularVelocity = 0f;
            transform.rotation = Quaternion.Euler(0,0,180);
            isRotationDone = true;
        }
        else if(rotation < 271f && rotation > 269f)
        {
            m_rb.angularVelocity = 0f;
            transform.rotation = Quaternion.Euler(0,0,270);
            isRotationDone = true;
        }
        if(isRotationDone)
        {
            m_hasFriction.enabled = true;
            m_rb.mass = m_initialMass;
        }     
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_playerInPushingRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.layer == Player.PLAYER_LAYER)
        {
            m_playerInPushingRange = false;
        }
    }

    public bool IsPlayerInPushingRange()
    {
        return m_playerInPushingRange;
    }

    public void Push(int dir)
    {
        if(!m_isPushed || dir != m_direction)
        {
            m_direction = dir;
            m_rb.velocity = new Vector2(m_direction*m_speed, m_rb.velocity.y);
            OnBoxPushStart?.Invoke(this, EventArgs.Empty);
            m_isBoxMoving = true;
            m_isPushed = true;
        }
    }

    public void StopPush()
    {
        if(m_isPushed)
        {
            OnBoxPushStop?.Invoke(this, EventArgs.Empty);
            m_isBoxMoving = false;
            m_rb.velocity = new Vector2(0f, m_rb.velocity.y);
            m_isPushed = false;
        }
    }

    public bool GetHasBeenGrabbed()
    {
        return m_hasBeenGrabbed;
    }

    public struct Anchor
    {
        public Vector3 localPosition;
        public float localRotation;
        public Rigidbody2D rigidbody;
    }

    private void HandlePush()
    {
        if(m_isPushed)
        {
            m_rb.AddForce(new Vector2(m_direction*m_speed*5, 0f), ForceMode2D.Impulse);
            if(m_rb.velocity.x > m_speed)
            {
                m_rb.velocity = new Vector2(m_speed, m_rb.velocity.y);
            }
            else if(m_rb.velocity.x < -m_speed)
            {
                m_rb.velocity = new Vector2(-m_speed, m_rb.velocity.y);
            }
            if(Mathf.Abs(m_rb.velocity.x) <= 1f && m_isBoxMoving)
            {
                m_isBoxMoving = false;
                OnBoxPushStop?.Invoke(this, EventArgs.Empty);
            }
            else if(Mathf.Abs(m_rb.velocity.x) >= 1f && !m_isBoxMoving)
            {
                m_isBoxMoving = true;
                OnBoxPushStart?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public Utils.GroundStable IsGroundStable()
    {
        bool isLeftGround = m_boxColGroundLeft.IsTouchingLayers(m_groundLayerMask);
        bool isRightGround = m_boxColGroundRight.IsTouchingLayers(m_groundLayerMask);
        bool isGroundNegative = m_boxColGroundNegative.IsTouchingLayers(m_groundLayerMask);

        if(isLeftGround && isRightGround && !isGroundNegative)
        {
            return Utils.GroundStable.GroundIsStable;
        }
        if(isGroundNegative)
        {
            return Utils.GroundStable.GroundIsUnstable;
        }
        return Utils.GroundStable.NoGround;
    }



        /*
    public void GetGrabbedBy(HandleGrabable handleGrabable)
    {
        /*if(!m_isGrabbed)
        {
            foreach(GameObject anchorGameObject in m_anchorPoints)
            {
                Collider2D anchorCollider = anchorGameObject.GetComponent<Collider2D>();
                if(anchorCollider.IsTouching(e.collider))
                {
                    Anchor newAnchor;
                    newAnchor.localPosition = anchorGameObject.transform.localPosition;
                    newAnchor.localRotation = anchorGameObject.transform.rotation.eulerAngles.z;
                    newAnchor.rigidbody = m_rb;
                    e.handleGrabable.SetAnchor(newAnchor);
                    e.handleGrabable.SetIsGrabbing(true);
                    m_rb.mass = m_massWhenGrabbed;
                    m_isGrabbed = true;
                    m_hasFriction.enabled = false;
                    m_rb.freezeRotation = false;
                    m_hasBeenGrabbed = true;
                }
            }
        }
        else
        {
            //e.handleGrabable.SetAnchor(newAnchor);
            e.handleGrabable.SetIsGrabbing(false);
            m_isGrabbed = false;
        }
    }*/


}
