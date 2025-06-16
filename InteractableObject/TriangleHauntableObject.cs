using UnityEngine;

public class TriangleHauntableObject : HauntableObject, IColliderDetect
{
    private Rigidbody2D m_rb;
    private int m_facingDirection;
    //Movement
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_deceleration;

    //Dash
    [SerializeField] private float m_dashingUnderPower;

    protected override void Awake()
    {
        base.Awake();
        m_facingDirection = 1;
    }
    protected override void Start()
    {
        base.Start();
        m_hauntableObjectAnimator.IdleState();
        m_rb = GetComponent<Rigidbody2D>();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(m_isToProcessUpdate)
        {
            CheckIfFlip();
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float horizontalInput = GameInput.Instance.xInput;

        if(horizontalInput < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if(horizontalInput > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }

        float velPower = 0.9f;
        float targetSpeed = horizontalInput * m_maxSpeed;
        float speedDif = targetSpeed - m_rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? m_acceleration : m_deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        m_rb.AddForce(movement*Vector2.right*10);
    }

    public void CheckIfFlip()
    {
        float xInput = GameInput.Instance.xInput;
        if(xInput != 0 && Mathf.Sign(xInput) != m_facingDirection)
        {
            Flip();
            CameraFollowObject.Instance.CallTurn();
        }
    }

    public void Flip()
    {
        m_facingDirection *= -1;
        m_rb.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public int GetFacingDirection()
    {
        return m_facingDirection;
    }

    public override void PlayerHauntCancel()
    {
        base.PlayerHauntCancel();
    }

    public override void PlayerHauntStart()
    {
        base.PlayerHauntStart();
    }

    public override void PlayerUnhauntCancel()
    {
        base.PlayerHauntStart();
    }

    public void ColliderDetected()
    {
        
    }

}
