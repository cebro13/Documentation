using UnityEngine;
using System;

public class HauntableSlider : HauntableObject
{
    public event EventHandler<OnHauntableSliderChangeEventArg> OnHauntableSliderChange;

    public class OnHauntableSliderChangeEventArg : EventArgs
    {
        public float valueNormalized;
    }

    [Header("Bottom or Left is 0")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_startSliderValue;
    [SerializeField] private bool m_useVerticalInput;
    //Movement
    [SerializeField] private float m_sliderMaxSpeed;
    [SerializeField] private float m_acceleration;
    [SerializeField] private float m_deceleration;

    [SerializeField] private Transform m_limitUpOrRight;
    [SerializeField] private Transform m_limitDownOrLeft;

    private Rigidbody2D m_rb;

    private float m_limitUpOrRightPos;
    private float m_limitDownOrLeftPos;

    private float m_maxDistance;

    protected override void Awake()
    {
        base.Awake();

        if(m_useVerticalInput)
        {
            m_limitUpOrRightPos = m_limitUpOrRight.position.y;
            m_limitDownOrLeftPos = m_limitDownOrLeft.position.y;
            transform.position = new Vector2(transform.position.x, m_limitDownOrLeftPos + m_maxDistance * m_startSliderValue);
        }
        else
        {
            m_limitUpOrRightPos = m_limitUpOrRight.position.x;
            m_limitDownOrLeftPos = m_limitDownOrLeft.position.x;
            transform.position = new Vector2(m_limitDownOrLeftPos + m_maxDistance * m_startSliderValue, transform.position.y);
        }
        
        m_maxDistance = m_limitUpOrRightPos - m_limitDownOrLeftPos;
    }


    protected override void Start()
    {
        base.Start();
        m_hauntableObjectAnimator.IdleState();
        m_rb = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.FixedUpdate();
        if(m_rb.velocity.magnitude > 0.1f)
        {
            OnHauntableSliderChange?.Invoke(this, new OnHauntableSliderChangeEventArg{valueNormalized = GetSliderValue()});
        }
    }

    public float GetSliderValue()
    {
        float normalizedValue;
        if(m_useVerticalInput)
        {
            normalizedValue = (m_limitUpOrRightPos - transform.position.y) / m_maxDistance;
        }
        else
        {
            normalizedValue = (m_limitUpOrRightPos - transform.position.x) / m_maxDistance;
        }
        return normalizedValue;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(m_isToProcessUpdate)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        if(m_useVerticalInput)
        {
            float verticalInput = GameInput.Instance.yInput;
            if(m_rb.position.y > m_limitUpOrRightPos && verticalInput >= 0)
            {
                m_rb.velocity = Vector2.zero;
                return;
            }
            else if(m_rb.position.y < m_limitDownOrLeftPos && verticalInput <= 0)
            {
                m_rb.velocity = Vector2.zero;
                return;
            }
            float velPower = 0.9f;
            float targetSpeed = verticalInput * m_sliderMaxSpeed;
            float speedDif = targetSpeed - m_rb.velocity.y;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? m_acceleration : m_deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            m_rb.AddForce(movement*Vector2.up*10);
        }
        else
        {
            float horizontalInput = GameInput.Instance.xInput;
            if(m_rb.position.x > m_limitUpOrRightPos && horizontalInput >= 0)
            {
                m_rb.velocity = Vector2.zero;
                return;
            }
            else if(m_rb.position.x < m_limitDownOrLeftPos && horizontalInput <= 0)
            {
                m_rb.velocity = Vector2.zero;
                return;
            }
            float velPower = 0.9f;
            float targetSpeed = horizontalInput * m_sliderMaxSpeed;
            float speedDif = targetSpeed - m_rb.velocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? m_acceleration : m_deceleration;
            float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
            m_rb.AddForce(movement*Vector2.right*10);
        }


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
