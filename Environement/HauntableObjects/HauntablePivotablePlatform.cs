using UnityEngine;

public class HauntablePivotablePlatform : HauntableObject
{
    private Rigidbody2D m_rb;
    //Movement
    [SerializeField] private float m_maxRotation;
    [SerializeField] private float m_startAtRotation;
    [SerializeField] private float m_force;

    //Dash
    [SerializeField] private Transform m_positionRightSide;
    [SerializeField] private Transform m_positionLeftSide;

    protected override void Awake()
    {
        base.Awake();
        HingeJoint2D hingeJoint2D = GetComponent<HingeJoint2D>();
        JointAngleLimits2D hingeLimits = hingeJoint2D.limits;
        hingeLimits.max = m_maxRotation;
        hingeLimits.min = -m_maxRotation;
        hingeJoint2D.useLimits = true;
        hingeJoint2D.limits = hingeLimits;
    }
    protected override void Start()
    {
        base.Start();
        m_rb = GetComponent<Rigidbody2D>();
        if(m_startAtRotation > m_maxRotation)
        {
            Debug.LogError("Le champ startAtRotation est plus grand que le champ maxRotation.");
        }
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.x, m_startAtRotation);
    }

    protected override void FixedUpdate()
    {
        base.Update();
        if(m_isToProcessUpdate)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = GameInput.Instance.xInput;

        if(horizontalInput < 0)
        {
            m_rb.AddForceAtPosition(-m_force*Vector2.up, m_positionLeftSide.position);
        }
        else if(horizontalInput > 0)
        {
            m_rb.AddForceAtPosition(-m_force*Vector2.up, m_positionRightSide.position);
        }
    }
}
