using UnityEngine;


public class ConspiBossHauntableObj_2 : HauntableObject, IColliderDetect
{
    private Rigidbody2D m_rb;
    //Movement
    [SerializeField] private float m_maxSpeed;
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_deceleration;

    [SerializeField] private Transform m_limitUp;
    [SerializeField] private Transform m_limitDown;

    private Vector2 m_limitUpPos;
    private Vector2 m_limitDownPos;

    protected override void Awake()
    {
        base.Awake();
        m_limitUpPos = m_limitUp.position;
        m_limitDownPos = m_limitDown.position;
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
        if(Player.Instance.playerStateMachine.CurrentState == Player.Instance.CinematicState)
        {
            m_rb.velocity = Vector2.zero;
            return;
        }
        if(m_isToProcessUpdate)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        float verticalInput = GameInput.Instance.yInput;
        if(m_rb.position.y > m_limitUpPos.y && verticalInput >= 0)
        {
            m_rb.velocity = Vector2.zero;
            return;
        }
        else if(m_rb.position.y < m_limitDownPos.y && verticalInput <= 0)
        {
            m_rb.velocity = Vector2.zero;
            return;
        }

        float velPower = 0.9f;
        float targetSpeed = verticalInput * m_maxSpeed;
        float speedDif = targetSpeed - m_rb.velocity.y;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? m_acceleration : m_deceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        m_rb.AddForce(movement*Vector2.up*10);
    }

    public override void PlayerHauntCancel()
    {
        base.PlayerHauntCancel();
    }

    public override void PlayerHauntStart()
    {
        base.PlayerHauntStart();
        Player.Instance.HauntingState.SetCanUnhaunt(false);
    }

    public override void PlayerUnhauntCancel()
    {
        base.PlayerHauntStart();
    }

    public void ColliderDetected()
    {
        
    }

}
